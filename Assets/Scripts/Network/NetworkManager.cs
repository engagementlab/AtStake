#undef DEBUG
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

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

	void Awake () {
		settings = new Settings (2, false, 3f, 3);
		MasterServer.ipAddress = "54.149.47.87";
		MasterServer.port = 23466;
		Network.natFacilitatorIP = "54.149.47.87";
		Network.natFacilitatorPort = 50005;
		MasterServer.ClearHostList ();
	}

	/**
	 *	Hosting
	 */

	public void HostGame (string instanceGameName) {
		StartServer (instanceGameName);
	}

	void StartServer (string gameInstanceName) {
		if (settings.secureServer)
			Network.InitializeSecurity ();

		// Use NAT punchthrough if no public IP present
		Network.InitializeServer (settings.maxConnections, 25001, !Network.HavePublicAddress ());
		MasterServer.RegisterHost (gameName, gameInstanceName);

		StartCoroutine (Test ());
	}

	public void StopServer () {
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
		MasterServer.ClearHostList ();
		StartCoroutine (FindHostsWrapper ());
	}

	IEnumerator FindHostsWrapper () {
		int attempts = settings.attempts;
		float timeout = settings.timeoutDuration;
		while (attempts > 0) {
			attempts --;
			yield return StartCoroutine (FindHosts (timeout, attempts == 0));
		}
	}

	IEnumerator FindHosts (float timeout, bool finalAttempt) {
			
		MasterServer.RequestHostList (gameName);

		while (hosts.Length == 0 && timeout > 0f) {
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
			if (host.playerLimit != 0 && host.playerLimit != -1 && host.connectedPlayers < host.playerLimit) {
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

	/**
	 *	Debugging
	 */

	IEnumerator Test () {
		
		float timeout = 1000f;
		ConnectionTesterStatus status = Network.TestConnection ();

		while (status == ConnectionTesterStatus.Undetermined && timeout > 0f) {
			timeout -= Time.deltaTime;
			status = Network.TestConnection ();
			yield return null;
		}

		#if UNITY_EDITOR && DEBUG
		Debug.Log (status);
		#endif
	}

	#if UNITY_EDITOR && DEBUG
	void OnMasterServerEvent (MasterServerEvent e) {
		if (e == MasterServerEvent.RegistrationSucceeded) {
			Debug.Log ("Registered game at " + MasterServer.ipAddress);
			return;
		}
		Debug.Log (e);
	}

	void OnFailedToConnectToMasterServer (NetworkConnectionError info) {
		Debug.Log ("Could not connect to Master Server: " + info);
	}
	#endif
}
