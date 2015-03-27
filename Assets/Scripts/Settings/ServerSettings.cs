#define DEBUG
using UnityEngine;
using System.Collections;

public static class ServerSettings {

	#if DEBUG
		public static readonly string IP = MasterServer.ipAddress;
		public static readonly int MasterServerPort = MasterServer.port;
		public static readonly int FacilitatorPort = Network.natFacilitatorPort;
	#else
		public static readonly string IP = "54.149.47.87";
		public static readonly int MasterServerPort = 23466;
		public static readonly int FacilitatorPort = 50005;
	#endif
}
