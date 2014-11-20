using UnityEngine;
using System.Collections;

public class NetworkManager : MonoBehaviour {

	readonly string gameName = "@Stake";
	HostData[] hosts = new HostData[0];

	struct Settings {

		public int maxConnections;
		public bool secureServer;
		public float timeoutDuration;

		public Settings (int maxConnections = 6, bool secureServer = false, float timeoutDuration = 10f) {
			this.maxConnections = maxConnections;
			this.secureServer = secureServer;
			this.timeoutDuration = timeoutDuration;
		}
	}
	Settings settings;

	// Debugging
	bool connected = false;
	string testMessage = "";

	void Awake () {
		settings = new Settings (6, false, 10f);
		MasterServer.ClearHostList ();
	}

	// Hosting

	public void HostGame (string instanceGameName) {
		StartServer (instanceGameName);
	}

	void StartServer (string gameInstanceName) {
		if (settings.secureServer)
			Network.InitializeSecurity ();

		// Use NAT punchthrough if no public IP present
		Network.InitializeServer (settings.maxConnections, 25001, !Network.HavePublicAddress ());
		MasterServer.RegisterHost (gameName, gameInstanceName);
	}

	public void StopServer () {
		Network.Disconnect ();
		MasterServer.UnregisterHost ();
		ResetHosts ();
	}

	// Joining

	public void JoinGame () {
		RefreshHostList ();
	}

	void RefreshHostList () {
		MasterServer.RequestHostList (gameName);
		StartCoroutine (FindHosts ());
	}

	IEnumerator FindHosts () {

		// time in seconds before we give up on finding a new game
		float timeout = settings.timeoutDuration;

		while (hosts.Length == 0 && timeout > 0f) {
			hosts = MasterServer.PollHostList ();
			timeout -= Time.deltaTime;
			yield return null;
		}

		if (timeout <= 0f) {
			OnTimeout ();
		} else {
			OnFoundGames ();
		}
		MasterServer.ClearHostList ();
	}

	void OnTimeout () {
		Events.instance.Raise (new JoinTimeoutEvent ());
	}

	void OnFoundGames () {
		Events.instance.Raise (new FoundGamesEvent (hosts));
	}

	public void ConnectToHost (HostData host) {
		Network.Connect (host);
	}

	public void DisconnectFromHost () {
		/*if (Network.connections.Length > 0)
			Network.CloseConnection (Network.connections[0], false);*/
		ResetHosts ();
	}

	void OnConnectedToServer () {
		connected = true;
		Events.instance.Raise (new ConnectedToServerEvent ());
	}

	void ResetHosts () {
		hosts = new HostData[0];
	}
}
