using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent (typeof(NetworkView))]
public class MultiplayerManager : MonoBehaviour {

	public NetworkManager networkManager;
	public MessageRelayer messageRelayer;

	string playerName = "";
	string PlayerName {
		get {
			// cache the name
			if (playerName == "") {
				playerName = Player.instance.Name;
			}
			return playerName;
		}
	}

	public bool Hosting {
		get { return player is GameHost; }
	}

	public int PlayerCount {
		get { return player.PlayerCount; }
	}

	public List<string> Players {
		get { return player.Players; }
	}

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
		messageRelayer = Instantiate (messageRelayer) as MessageRelayer;
	}

	/**
	 *	Actions
	 */

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
		player = new GameHost (PlayerName);
		networkManager.HostGame (PlayerName);
		SendRefreshListMessage ();
	}

	public void JoinGame () {
		player = new GameClient (PlayerName);
		networkManager.JoinGame ();
	}

	public void ConnectToHost (HostData host) {
		networkManager.ConnectToHost (host);
	}

	public void ExitLobby () {
		if (player is GameHost) {
			networkManager.StopServer ();
		} else {
			networkView.RPC ("UnregisterPlayer", RPCMode.Server, player.playerName);
			networkManager.DisconnectFromHost ();
		}
	}

	/**
	 *	Messages
	 */
	
	void OnJoinTimeoutEvent (JoinTimeoutEvent e) {
		
	}

	void OnFoundGamesEvent (FoundGamesEvent e) {
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

	/**
	 *	RPCs
	 */

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

	/**
	 *	Debugging
	 */

	public void DebugCreateClient () {
		player = new GameClient (PlayerName);
	}

	public void DebugCreateHost () {
		player = new GameHost (PlayerName);	
	}
}
