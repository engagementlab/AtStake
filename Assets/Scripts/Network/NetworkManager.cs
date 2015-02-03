using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class NetworkManager : MonoBehaviour {

	readonly string gameName = "@Stake";
	HostData[] hosts = new HostData[0];

	struct Settings {

		public int maxConnections;
		public bool secureServer;
		public float timeoutDuration;

		public Settings (int maxConnections = 5, bool secureServer = false, float timeoutDuration = 10f) {
			this.maxConnections = maxConnections;
			this.secureServer = secureServer;
			this.timeoutDuration = timeoutDuration;
		}
	}
	Settings settings;

	void Awake () {
		settings = new Settings (2, false, 15f);
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
		Network.InitializeServer (settings.maxConnections, 25003, !Network.HavePublicAddress ());
		MasterServer.RegisterHost (gameName, gameInstanceName);
	}

	public void StopServer () {
		Network.Disconnect ();
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
		RefreshHostList ();
	}

	void RefreshHostList () {
		MasterServer.RequestHostList (gameName);
		StartCoroutine (FindHosts ());
	}

	IEnumerator FindHosts () {

		ResetHosts ();
		OnFoundGames ();
		
		// TODO: This doesn't seem to be working
		int attempts = 3;
		float timeout = settings.timeoutDuration / attempts;

		for (int i = 0; i < attempts; i ++) {
			while (hosts.Length == 0 && timeout > 0f) {
				hosts = MasterServer.PollHostList ();
				timeout -= Time.deltaTime;
				yield return null;
			}
			if (timeout <= 0f) {
				if (i == attempts-1) {
					OnTimeout ();
					break;
				} else {
					MasterServer.RequestHostList (gameName);
				}
			} else {
				OnFoundGames ();
				break;
			}
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
			if (host.playerLimit != 0 && host.connectedPlayers < host.playerLimit) {
				joinable.Add (host);
			}
		}
		Events.instance.Raise (new FoundGamesEvent (joinable.ToArray ()));
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

	void OnConnectedToServer () {
		Events.instance.Raise (new ConnectedToServerEvent ());
	}

	void ResetHosts () {
		hosts = new HostData[0];
	}
}
