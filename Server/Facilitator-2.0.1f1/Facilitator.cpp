#include "Utility.h"
#include "Log.h"
#include "TableSerializer.h"
#include "BitStream.h"
#include "StringCompressor.h"
#include "DS_Table.h"
#include "RakPeerInterface.h"
#include "RakNetworkFactory.h"
#include "RakSleep.h"
#include "MessageIdentifiers.h"
#include "NatPunchthroughServer.h"
#include <signal.h>
#include "SocketLayer.h"
#include <stdarg.h>

#ifdef WIN32
#include <stdio.h>
#include <time.h>
#include <windows.h>
#else
#include <stdlib.h>
#include <time.h>
#include <unistd.h>
#endif

NatPunchthroughServer natPunchthrough;
bool quit;
bool printStats = false;

char* logfile = "facilitator.log";
const int fileBufSize = 1024;
char pidFile[fileBufSize];

void shutdown(int sig)
{
	Log::print_log("Shutting down\n\n");
	quit = true;
}

void usage()
{
	printf("\nAccepted parameters are:\n\t"
		   "-p\tListen port (1-65535)\n\t"
		   "-d\tDaemon mode, run in the background\n\t"
		   "-l\tUse given log file\n\t"
		   "-e\tDebug level (0=OnlyErrors, 1=Warnings, 2=Informational(default), 2=FullDebug)\n\t"
		   "-c\tMax connection count. [1000]\n\t"
		   "-s\tStatistics print delay (in seconds). [0]\n\n"
		   "If any parameter is omitted the default value is used.\n");
}

void ProcessPacket(Packet *packet)
{
	switch (packet->data[0])
	{
		case ID_DISCONNECTION_NOTIFICATION:
			Log::info_log("%s has diconnected\n", packet->systemAddress.ToString());
			break;
		case ID_CONNECTION_LOST:
			Log::warn_log("Connection to %s lost\n", packet->systemAddress.ToString());
			break;
		case ID_NO_FREE_INCOMING_CONNECTIONS:
			Log::warn_log("No free incoming connection for %s\n", packet->systemAddress.ToString());
			break;
		case ID_NEW_INCOMING_CONNECTION:
			Log::info_log("New connection established to %s (%s)\n", packet->systemAddress.ToString(), packet->guid.ToString());
			break;
		case ID_CONNECTION_REQUEST_ACCEPTED:
			Log::info_log("Connection to %s accepted\n", packet->systemAddress.ToString());
			break;
		case ID_CONNECTION_ATTEMPT_FAILED:
			Log::warn_log("Connection attempt to %s failed\n", packet->systemAddress.ToString());
			break;
		case ID_NAT_TARGET_NOT_CONNECTED:
		{
			SystemAddress systemAddress;
			RakNet::BitStream b(packet->data, packet->length, false);
			b.IgnoreBits(8); // Ignore the ID_...
			b.Read(systemAddress);
			Log::warn_log("ID_NAT_TARGET_NOT_CONNECTED to %s\n", systemAddress.ToString());
			break;
		}
		case ID_NAT_CONNECTION_TO_TARGET_LOST:
		{
			SystemAddress systemAddress;
			RakNet::BitStream b(packet->data, packet->length, false);
			b.IgnoreBits(8); // Ignore the ID_...
			b.Read(systemAddress);
			Log::warn_log("ID_NAT_CONNECTION_TO_TARGET_LOST to %s\n", systemAddress.ToString());
			break;
		}
		default:
			Log::error_log("Unknown ID %d from %s\n", packet->data[0], packet->systemAddress.ToString());
	}
}

int main(int argc, char *argv[])
{  
#ifndef WIN32
	setlinebuf(stdout);
#endif

	RakPeerInterface *peer = RakNetworkFactory::GetRakPeerInterface();	// The facilitator
	
	// Default values
	int facilitatorPort = 50005;
	int connectionCount = 1000;

	time_t timerInterval = 10;	// 10 seconds
	time_t rotateCheckTimer = time(0) + timerInterval;
	int rotateSizeLimit = 5000000;	// 5 MB
	bool useLogFile = false;
	int statDelay = 30;
	bool daemonMode = false;
			
	// Process command line arguments
	for (int i = 1; i < argc; i++)
	{
		if (strlen(argv[i]) == 2 && argc>=i+1)
		{
			switch (argv[i][1]) 
			{
				case 'd':
				{
					daemonMode = true;
					break;
				}
				case 'p':
				{
					facilitatorPort = atoi(argv[i+1]);
					i++;
					if (facilitatorPort < 1 || facilitatorPort > 65535)
					{
						fprintf(stderr, "Facilitator port is invalid, should be between 0 and 65535.\n");
						return 1;
					}
					break;
				}
				case 'c':
				{
					connectionCount = atoi(argv[i+1]);
					i++;
					if (connectionCount < 0)
					{
						fprintf(stderr, "Connection count must be higher than 0.\n");
						return 1;
					}
					break;
				}
				case 'l':
				{
					useLogFile = Log::EnableFileLogging(logfile);
					break;
				}
				case 'e':
				{
					int debugLevel = atoi(argv[i+1]);
					Log::sDebugLevel = debugLevel;
					i++;
					if (debugLevel < 0 || debugLevel > 9)
					{
						fprintf(stderr, "Log level can be 0(errors), 1(warnings), 2(informational), 9(debug)\n");
						return 1;
					}
					break;
				}
				case 's':
				{
					int statDelay =  atoi(argv[i+1]);
					i++;
					if (statDelay < 0)
					{
						fprintf(stderr, "Statistics print delay must not be lower than 0. Use 0 to disable.\n");
						return 1;
					}
					Log::print_log("NOT IMPLEMENTED");
					printStats = true;
					break;
				}
				case '?':
				{
					usage();
					return 0;
				}
				default:
					fprintf(stderr, "Parsing error, unknown parameter '-%c'\n\n", argv[i][1]);
					usage();
					return 1;
			}
		}
		else
		{
			printf("Parsing error, incorrect parameters\n\n");
			usage();
			return 1;
		}
	}
#ifndef WIN32
	if (daemonMode)
	{
		printf("Running in daemon mode, file logging enabled...\n");
		if (!useLogFile)
			useLogFile = Log::EnableFileLogging(logfile);
		// Don't change cwd to /
		// Beware that log/pid files are placed wherever this was launched from
		daemon(1, 0);
	}
	
	if (!WriteProcessID(argv[0], &pidFile[0], fileBufSize))
		perror("Warning, failed to write own PID value to PID file\n");
#endif

	peer->SetMaximumIncomingConnections(connectionCount);
	char ipList[ MAXIMUM_NUMBER_OF_INTERNAL_IDS ][ 16 ];
	unsigned int binaryAddresses[MAXIMUM_NUMBER_OF_INTERNAL_IDS];
	SocketLayer::Instance()->GetMyIP( ipList, binaryAddresses );
	//SocketDescriptor sd(facilitatorPort,ipList[0]);
	SocketDescriptor sd(facilitatorPort,0);
	if (!peer->Startup(connectionCount, 30, &sd, 1))
	{
		Log::error_log("Failed to start RakPeer!\n");
		RakNetworkFactory::DestroyRakPeerInterface(peer);
		return 1;
	}
	peer->AttachPlugin(&natPunchthrough);
	
	Log::print_log("Unity Facilitator version 2.0.0\n");
	Log::print_log("Listen port set to %d\n",facilitatorPort);
	Log::print_log("%d connection count limit\n", connectionCount);
	if (printStats)
		Log::print_log("%d sec delay between statistics print to log\n", statDelay);
	
	// Register signal handlers
#ifndef WIN32
	if (signal(SIGHUP, Log::RotateLogFile) == SIG_ERR)
		Log::error_log("Problem setting up hangup signal handler");
	if (signal(SIGINT, shutdown) == SIG_ERR || signal(SIGTERM, shutdown) == SIG_ERR)
		Log::error_log("Problem setting up terminate signal handler");
	else
#endif
		Log::print_log("To quit press Ctrl-C\n----------------------------------------------------\n");

	Packet *p;
	while (!quit)
	{
		p=peer->Receive();
		while (p)
		{
			ProcessPacket(p);
			peer->DeallocatePacket(p);
			p=peer->Receive();
		}
		// Is it time to rotate the logs?
		if (useLogFile)
		{
			if (rotateCheckTimer < time(0)) {
				// We should always be writing to the end of the stream, so the current position should give the total size
				int position = Log::GetLogSize();
				if (position > rotateSizeLimit)
				{
					Log::error_log("Rotating logs, size limit reached (cur %d, limit %d)\n", position, rotateSizeLimit);
					Log::RotateLogFile(0);
				}
				rotateCheckTimer = time(0) + timerInterval;
			}
		}
		RakSleep(30);
	}

	if (pidFile)
	{
		if (remove(pidFile) != 0)
			fprintf(stderr, "Failed to remove PID file at %s\n", pidFile);
	}
	peer->Shutdown(100,0);
	RakNetworkFactory::DestroyRakPeerInterface(peer);
				
	return 0;
}
