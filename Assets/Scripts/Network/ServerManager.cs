#define DEBUG
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ServerManager : MonoBehaviour {

	readonly string gameName = "@Stake";
	HostData[] hosts = new HostData[0];
	bool hosting = false;

	Settings settings;
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

	TestState testState = TestState.Running;
	enum TestState {
		Running,
		Failed,
		Succeeded
	}

	void Awake () {

		settings = new Settings (4, false, 3f, 3);

		/*MasterServer.ipAddress = ServerSettings.IP;
		MasterServer.port = ServerSettings.MasterServerPort;
		Network.natFacilitatorIP = ServerSettings.IP;
		Network.natFacilitatorPort = ServerSettings.FacilitatorPort;*/

		TestConnection ();
	}

	/**
	 *	Hosting
	 */

	public void HostGame (string gameInstanceName) {
		hosting = true;
		if (settings.secureServer)
			Network.InitializeSecurity ();

		// Use NAT punchthrough if no public IP present
		Network.InitializeServer (settings.maxConnections, 25001, !Network.HavePublicAddress ());
		MasterServer.RegisterHost (gameName, gameInstanceName);
	}

	public void DisconnectHost () {
		Network.Disconnect ();
		Network.maxConnections = settings.maxConnections;
		MasterServer.UnregisterHost ();
		ResetHosts ();
	}

	public void StartGame () {
		Network.maxConnections = -1;
	}

	/**
	 *	Joining
	 */

	public void JoinGame () {
		hosting = false;
		MasterServer.ClearHostList ();
		StartCoroutine (FindHostsWrapper ());
	}

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
			if (host.gameName != "TestConnection" &&
				host.playerLimit != 0 && 
				host.playerLimit != -1 && 
				host.connectedPlayers < host.playerLimit-1) {
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
		ResetHosts ();
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

	void OnDisconnectedFromServer (NetworkDisconnection info) {
		if (!hosting) {
			DisconnectFromHost ();
			Events.instance.Raise (new DisconnectedFromServerEvent ());
		}
	}

	/**
	 *	Testing connection
	 */

	void TestConnection () {
		Network.InitializeServer (settings.maxConnections, 25001, !Network.HavePublicAddress ());
	}

	void OnServerInitialized () {
		if (testState == TestState.Running) {
			MasterServer.RegisterHost (gameName, "TestConnection");
		}
	}

	void OnFailedToConnectToMasterServer (NetworkConnectionError info) {
		if (testState == TestState.Running) {
			testState = TestState.Failed;
			Events.instance.Raise (new ServerUnavailableEvent ());
			#if DEBUG
				Debug.Log ("Server unavailable");
			#endif
		}
	}

	void OnMasterServerEvent (MasterServerEvent e) {
		if (e == MasterServerEvent.RegistrationSucceeded) {
			if (testState == TestState.Running) {
				testState = TestState.Succeeded;
				Events.instance.Raise (new ServerAvailableEvent ());
				Network.Disconnect ();
				#if DEBUG
					Debug.Log ("Server available");
				#endif
			}
		}
	}
}
