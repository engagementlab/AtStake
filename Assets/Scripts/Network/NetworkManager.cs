#define DEBUG
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum ConnectionType {
	None,
	Wifi,
	Bluetooth
}

public class NetworkManager : MonoBehaviour {

	readonly string gameName = "@Stake";
	HostData[] hosts = new HostData[0];

	struct Settings {

		public readonly int maxConnections;
		public readonly bool secureServer;
		public readonly float timeoutDuration;
		public readonly int attempts;

		public Settings (int maxConnections=5, bool secureServer=false, float timeoutDuration=10f, int attempts=3) {
			this.maxConnections = maxConnections;
			this.secureServer = secureServer;
			this.timeoutDuration = timeoutDuration;
			this.attempts = attempts;
		}
	}
	
	Settings settings;
	bool hosting = false;
	ConnectionTesterStatus connectionStatus = ConnectionTesterStatus.Undetermined;
	bool ServerRunning {
		get { 
			if (Application.internetReachability == NetworkReachability.NotReachable) {
				return false;
			}
			return (connectionStatus != ConnectionTesterStatus.Undetermined &&
					connectionStatus != ConnectionTesterStatus.PublicIPNoServerStarted &&
					connectionStatus != ConnectionTesterStatus.Error &&
					connectionStatus != ConnectionTesterStatus.PublicIPPortBlocked);
		}
	}

	ConnectionType connectionType = ConnectionType.None;
	public ConnectionType ConnectionType {
		get { return connectionType; }
	}

	void Awake () {
		settings = new Settings (5, false, 3f, 3);

		// Wifi
		/*MasterServer.ipAddress = ServerSettings.IP;
		MasterServer.port = ServerSettings.MasterServerPort;
		Network.natFacilitatorIP = ServerSettings.IP;
		Network.natFacilitatorPort = ServerSettings.FacilitatorPort;*/
		MasterServer.ClearHostList ();
		//StartCoroutine (CoTestConnection ()); // this will always be 'PublicIPIsConnectable'

		// Bluetooth
		MultiPeerManager.peerDidChangeStateToConnectedEvent += peerDidChangeStateToConnectedEvent;
	}

	void AdvertiseDevice () {
		MultiPeer.advertiseCurrentDeviceWithNearbyServiceAdvertiser (true, "atstake");
	}

	/**
	 *	Hosting
	 */

	public void HostGame (string instanceGameName) {
		/*if (ServerRunning) {
			StartServer (instanceGameName);
		} else {
			StartBluetoothHost ();
		}*/
		StartServer (instanceGameName);
		hosting = true;
	}

	// Bluetooth

	void StartBluetoothHost () {
		connectionType = ConnectionType.Bluetooth;
		AdvertiseDevice ();
		MultiPeer.showPeerPicker ();
	}

	// Wifi

	void StartServer (string gameInstanceName) {
		connectionType = ConnectionType.Wifi;
		if (settings.secureServer)
			Network.InitializeSecurity ();

		// Use NAT punchthrough if no public IP present
		Network.InitializeServer (settings.maxConnections, 25001, !Network.HavePublicAddress ());
		MasterServer.RegisterHost (gameName, gameInstanceName);
	}

	public void StopServer () {
		Network.Disconnect ();
		Network.maxConnections = settings.maxConnections;
		MasterServer.UnregisterHost ();
		ResetHosts ();
		connectionType = ConnectionType.None;
	}

	public void StartGame () {
		Network.maxConnections = -1;
	}

	/**
	 *	Joining
	 */

	public void JoinGame () {
		hosting = false;
		/*if (ServerRunning) {
			MasterServer.ClearHostList ();
			StartCoroutine (FindHostsWrapper ());
		} else {
			StartBluetoothJoin ();
		}*/
		MasterServer.ClearHostList ();
		StartCoroutine (FindHostsWrapper ());
	}

	void peerDidChangeStateToConnectedEvent (string param) {
		connectionType = ConnectionType.Bluetooth;
		Events.instance.Raise (new ConnectedToServerEvent ());
	}

	// Bluetooth

	void StartBluetoothJoin () {
		AdvertiseDevice ();
	}

	// Wifi

	IEnumerator FindHostsWrapper () {
		int attempts = settings.attempts;
		float timeout = settings.timeoutDuration;
		while (attempts > 0 && !hosting) {
			attempts --;
			yield return StartCoroutine (FindHosts (timeout, attempts == 0));
		}
	}

	IEnumerator FindHosts (float timeout, bool finalAttempt) {
			
		MasterServer.RequestHostList (gameName);

		while (hosts.Length == 0 && timeout > 0f && !hosting) {
			hosts = MasterServer.PollHostList ();
			timeout -= Time.deltaTime;
			yield return null;
		} 
		if (timeout <= 0f) {
			if (finalAttempt) OnTimeout ();
		} else {
			OnFoundGames ();
		}

		MasterServer.ClearHostList ();
	}

	void OnTimeout () {
		Events.instance.Raise (new JoinTimeoutEvent ());
	}

	void OnFoundGames () {
		List<HostData> joinable = new List<HostData> ();
		for (int i = 0; i < hosts.Length; i ++) {
			HostData host = hosts[i];
			if (host.playerLimit != 0 && host.playerLimit != -1 && host.connectedPlayers < host.playerLimit-1) {
				joinable.Add (host);
			}
		}
		if (joinable.Count > 0) {
			Events.instance.Raise (new FoundGamesEvent (joinable.ToArray ()));
		} else {
			OnTimeout ();
		}
	}

	public void ConnectToHost (HostData host) {
		Network.Connect (host);
	}

	public void DisconnectFromHost () {
		if (Network.isServer) {
			StopServer ();
		} else {
			ResetHosts ();
		}
		connectionType = ConnectionType.None;
	}

	void ResetHosts () {
		hosts = new HostData[0];
	}

	/**
	 *	Events
	 */

	void OnConnectedToServer () {
		Events.instance.Raise (new ConnectedToServerEvent ());
	}

	IEnumerator CoTestConnection () {
		
		float timeout = 30f; // # of seconds to test for a connection
		ConnectionTesterStatus status = Network.TestConnection ();

		while (status == ConnectionTesterStatus.Undetermined && timeout > 0f) {
			timeout -= Time.deltaTime;
			status = Network.TestConnection ();
			yield return null;
		}

		connectionStatus = status;
		Debug.Log (connectionStatus);
	}

	void OnFailedToConnectToMasterServer (NetworkConnectionError info) {
		if (hosting) {
			StartBluetoothHost ();
		}
	}

	/**
	 *	Debugging
	 */


	#if UNITY_EDITOR && DEBUG

	int maxConnectionsCache = 0;
	int connectionsCache = 0;
	void Update () {
		if (maxConnectionsCache != Network.maxConnections) {
			maxConnectionsCache = Network.maxConnections;
			Debug.Log ("Max: " + maxConnectionsCache);
		}
		if (connectionsCache != Network.connections.Length) {
			connectionsCache = Network.connections.Length;
			Debug.Log ("Connections: " + connectionsCache);
		}
	}

	void OnMasterServerEvent (MasterServerEvent e) {
		if (e == MasterServerEvent.RegistrationSucceeded) {
			Debug.Log ("Registered game at " + MasterServer.ipAddress);
			return;
		}
		Debug.Log (e);
	}
	#endif
}
