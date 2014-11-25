using UnityEngine;
using System.Collections;

[RequireComponent (typeof(NetworkView))]
public class MultiplayerManager : MonoBehaviour {

	public NetworkManager networkManager;
	string playerName = "";
	public string PlayerName {
		get { return playerName; }
		set { playerName = value; }
	}

	public bool Hosting {
		get { return player is GameHost; }
	}

	bool findingGames = false;
	bool noGamesFound = false;
	HostData[] hosts;	
	GamePlayer player;

	public static MultiplayerManager instance;

	void Awake () {

		if (instance == null)
			instance = this;

		// Asynchronous events
		Events.instance.AddListener<JoinTimeoutEvent> (OnJoinTimeoutEvent);
		Events.instance.AddListener<FoundGamesEvent> (OnFoundGamesEvent);
		Events.instance.AddListener<ConnectedToServerEvent> (OnConnectedToServerEvent);
	}

	void Start () {
		networkManager = Instantiate (networkManager) as NetworkManager;
	}

	void OnGUI () {

		return;

		if (GameStateController.instance.State.name != "Multiplayer")
			return;
		string screen = GameStateController.instance.Screen.name;
		switch (screen) {
			case "Enter Name":
				GUILayout.Label ("Enter your name");
				playerName = GUILayout.TextField (playerName, 25);
				if (GUILayout.Button ("Back")) {
					playerName = "";
					GameStateController.instance.GotoState ("Start Screen");
				}
				if (playerName != "") {
					if (GUILayout.Button ("Enter")) {
						if (playerName != "") 
							GotoScreen ("Host or Join");
					}
				}
				break;
			case "Host or Join":
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
					GotoPreviousScreen ();
				}
				break;
			case "Games List":
				for (int i = 0; i < hosts.Length; i ++) {
					if (GUILayout.Button (hosts[i].gameName)) {
						ConnectToHost (hosts[i]);
					}
				}
				break;
			case "Lobby":
				
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

	void GotoScreen (string screenName) {
		GameStateController.instance.GotoScreen (screenName);
	}

	void GotoPreviousScreen () {
		GameStateController.instance.GotoPreviousScreen ();
	}

	string GetScreen () {
		return GameStateController.instance.Screen.name;
	}

	public void HostGame () {
		player = new GameHost (playerName);
		networkManager.HostGame (playerName);
		SendRefreshListMessage ();
		//GotoScreen ("Lobby");
	}

	public void JoinGame () {
		player = new GameClient (playerName);
		networkManager.JoinGame ();
		findingGames = true;
		noGamesFound = false;
	}

	public void ConnectToHost (HostData host) {
		networkManager.ConnectToHost (host);
		//GotoScreen ("Lobby");
	}

	public void ExitLobby () {
		if (player is GameHost) {
			networkManager.StopServer ();
			//GotoScreen ("Host or Join");
		} else {
			networkView.RPC ("UnregisterPlayer", RPCMode.Server, player.playerName);
			networkManager.DisconnectFromHost ();
			//GotoPreviousScreen ();
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
		GotoScreen ("Games List");
	}

	void OnConnectedToServerEvent (ConnectedToServerEvent e) {
		networkView.RPC ("RegisterPlayer", RPCMode.Server, player.playerName);
	}

	void OnDisconnectedFromServer (NetworkDisconnection info) {
		if (player is GameClient) {
			networkManager.DisconnectFromHost ();
			if (GetScreen () == "Lobby") {
				GotoScreen ("Host or Join");
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
		networkView.RPC ("SendRefreshListMessage", RPCMode.Others);
		SendRefreshListMessage ();
	}

	[RPC]
	void ClearPlayerList () {
		player.ClearPlayers ();
	}

	[RPC]
	void AddPlayer (string clientName) {
		player.AddPlayer (clientName);
	}

	[RPC]
	void SendRefreshListMessage() {
		Events.instance.Raise (new RefreshPlayerListEvent (player.GetPlayerNames ()));
	}
}
