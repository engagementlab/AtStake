using UnityEngine;
using System.Collections;

[RequireComponent (typeof(NetworkView))]
public class MultiplayerManager : MonoBehaviour {

	public NetworkManager networkManager;
	string playerName = "";
	bool findingGames = false;
	bool noGamesFound = false;
	HostData[] hosts;

	enum State {
		Start,
		EnterName,
		HostJoin,
		GamesList,
		Lobby
	}

	State state = State.Start;
	GamePlayer player;

	void Awake () {
		Events.instance.AddListener<JoinTimeoutEvent> (OnJoinTimeoutEvent);
		Events.instance.AddListener<FoundGamesEvent> (OnFoundGamesEvent);
		Events.instance.AddListener<ConnectedToServerEvent> (OnConnectedToServerEvent);
	}

	void Start () {
		networkManager = Instantiate (networkManager) as NetworkManager;
	}

	void OnGUI () {
		switch (state) {
			case State.Start: 
				if (GUILayout.Button ("Start Game")) {
					state = State.EnterName;
				}
				break;
			case State.EnterName:
				playerName = GUILayout.TextField (playerName, 25);
				if (GUILayout.Button ("Back")) {
					playerName = "";
					state = State.Start;
				}
				if (playerName != "") {
					if (GUILayout.Button ("Enter")) {
						if (playerName != "") 
							state = State.HostJoin;
					}
				}
				break;
			case State.HostJoin:
				if (noGamesFound) {
					GUILayout.Label ("no games found :(");
				}
				if (findingGames) {
					GUILayout.Label ("searching for games...");
					break;
				}
				if (GUILayout.Button ("Host")) {
					HostGame ();
				}
				if (GUILayout.Button ("Join")) {
					JoinGame ();
				}
				if (GUILayout.Button ("Back")) {
					state = State.EnterName;
				}
				break;
			case State.GamesList:
				for (int i = 0; i < hosts.Length; i ++) {
					if (GUILayout.Button (hosts[i].gameName)) {
						ConnectToHost (hosts[i]);
					}
				}
				break;
			case State.Lobby:

				GUILayout.Label ("Lobby");
				string[] names = player.GetPlayerNames ();
				for (int i = 0; i < names.Length; i ++) {
					GUILayout.Label (names[i]);
				}

				if (player is GameHost) {
					if (player.HasOtherPlayers ()) {
						if (GUILayout.Button ("Start game")) {

						}
					} else {
						GUILayout.Label ("waiting for other players to join");
					}
				} 
				if (GUILayout.Button ("Back")) {
					ExitLobby ();
				}
				break;
		}
	}

	// Actions

	void HostGame () {
		player = new GameHost (playerName);
		networkManager.HostGame (playerName);
		state = State.Lobby;
	}

	void JoinGame () {
		player = new GameClient (playerName);
		networkManager.JoinGame ();
		findingGames = true;
		noGamesFound = false;
	}

	void ConnectToHost (HostData host) {
		networkManager.ConnectToHost (host);
		state = State.Lobby;
	}

	void ExitLobby () {
		if (player is GameHost) {
			networkManager.StopServer ();
			state = State.HostJoin;
		} else {
			networkView.RPC ("UnregisterPlayer", RPCMode.Server, player.playerName);
			networkManager.DisconnectFromHost ();
			state = State.HostJoin;
		}
	}

	// Messages
	
	void OnJoinTimeoutEvent (JoinTimeoutEvent e) {
		findingGames = false;
		noGamesFound = true;
	}

	void OnFoundGamesEvent (FoundGamesEvent e) {
		findingGames = false;
		hosts = e.hosts;
		state = State.GamesList;
	}

	void OnConnectedToServerEvent (ConnectedToServerEvent e) {
		networkView.RPC ("RegisterPlayer", RPCMode.Server, player.playerName);
	}

	void OnDisconnectedFromServer (NetworkDisconnection info) {
		if (player is GameClient) {
			networkManager.DisconnectFromHost ();
			if (state == State.Lobby) {
				state = State.HostJoin;
			}
		}
	}

	// RPCs

	[RPC]
	void RegisterPlayer (string clientName) {
		player.AddPlayer (clientName);
		RefreshPlayerList ();
	}

	[RPC]
	void UnregisterPlayer (string clientName) {
		player.RemovePlayer (clientName);
		RefreshPlayerList ();
	}

	[RPC]
	void RefreshPlayerList () {
		networkView.RPC ("ClearPlayerList", RPCMode.Others);
		string[] names = player.GetPlayerNames ();
		for (int i = 0; i < names.Length; i ++) {
			networkView.RPC ("AddPlayer", RPCMode.Others, names[i]);
		}
	}

	[RPC]
	void ClearPlayerList () {
		player.ClearPlayers ();
	}

	[RPC]
	void AddPlayer (string clientName) {
		player.AddPlayer (clientName);
	}
}
