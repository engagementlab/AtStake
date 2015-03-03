using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// new multiplayer manager
[RequireComponent (typeof(NetworkView))]
public class MultiplayerManager2 : MonoBehaviour {

	public NetworkingManager networkingManager;
	public static MultiplayerManager2 instance;
	
	string playerName = "";
	PlayerList playerList = new PlayerList ();
	HostData hostAttempt = null;

	public bool Hosting { get; private set; }
	public bool Connected { get; private set; }
	public int PlayerCount {
		get { return playerList.Count; }
	}
	public List<string> Players {
		get { return playerList.Players; }
	}

	void Awake () {
		
		if (instance == null)
			instance = this;

		networkingManager = Instantiate (networkingManager) as NetworkingManager;
		Hosting = false;
		Connected = false;
		
		Events.instance.AddListener<EnterNameEvent> (OnEnterNameEvent);
		Events.instance.AddListener<FoundGamesEvent> (OnFoundGamesEvent);
		Events.instance.AddListener<ConnectedToServerEvent> (OnConnectedToServerEvent);
		Events.instance.AddListener<HostReceiveMessageEvent> (OnHostReceiveMessageEvent);
		Events.instance.AddListener<AllReceiveMessageEvent> (OnAllReceiveMessageEvent);
	}

	void GotoScreen (string screenName) {
		GameStateController.instance.GotoScreen (screenName);
	}

	public void Disconnect () {
		if (Hosting) {
			DisconnectHost ();
		} else {
			MessageSender.instance.SendMessageToHost ("UnregisterPlayer", playerName);
			DisconnectFromHost ();
		}
	}

	/**
	 *	Hosting
	 */

	public void StartGame () {
		networkingManager.StartGame ();
	}

	public void HostGame () {
		Hosting = true;
		networkingManager.HostGame (playerName);
		playerList.Init (playerName);
		RaiseRefreshPlayerList ();
	}

	void RequestRegistration (string clientName) {
		if (playerList.Add (clientName)) {
			MessageSender.instance.SendMessageToAll ("AcceptPlayer", clientName);
			RefreshPlayerList ();
			RaiseRefreshPlayerList ();
		} else {
			MessageSender.instance.SendMessageToAll ("RejectPlayer", clientName);
		}
	}

	void UnregisterPlayer (string clientName) {
		playerList.Remove (clientName);
		RefreshPlayerList ();
		RaiseRefreshPlayerList ();
	}

	void RefreshPlayerList () {
		MessageSender.instance.SendMessageToAll ("ClearPlayerList");
		for (int i = 0; i < playerList.Players.Count; i ++) {
			MessageSender.instance.SendMessageToAll ("AddPlayer", playerList.Players[i]);
		}
		MessageSender.instance.SendMessageToAll ("ListRefreshed");
	}

	void DisconnectHost () {
		networkingManager.DisconnectHost ();
	}

	/**
	 *	Joining
	 */

	public void JoinGame () {
		Hosting = false;
		networkingManager.JoinGame ();
	}

	public void ConnectToHost (HostData hostAttempt) {
		this.hostAttempt = hostAttempt;
		networkingManager.ConnectToHost (hostAttempt);
	}

	public void ConnectToHost () {
		if (hostAttempt != null) {
			networkingManager.ConnectToHost (hostAttempt);
		}
	}

	void DisconnectFromHost () {
		Connected = false;
		networkingManager.DisconnectFromHost ();
	}

	void AcceptPlayer (string clientName) {
		if (playerName == clientName) {
			Connected = true;
			Events.instance.Raise (new RegisterEvent ());
		}
	}

	void RejectPlayer (string clientName) {
		if (!Connected && !Hosting && playerName == clientName) {
			DisconnectFromHost ();
			Events.instance.Raise (new NameTakenEvent (playerName));
			playerName = "";
		}
	}

	void ClearPlayerList () {
		if (!Hosting) {
			playerList.Clear ();
		}
	}

	void AddPlayer (string playerName) {
		if (!Hosting) {
			playerList.Add (playerName);
		}
	}

	/**
	 *	Events
	 */

	void RaiseRefreshPlayerList () {
		Events.instance.Raise (new RefreshPlayerListEvent (playerList.Names));
	}

	void OnEnterNameEvent (EnterNameEvent e) {
		playerName = e.name;
	}

	void OnFoundGamesEvent (FoundGamesEvent e) {
		if (GameStateController.instance.Screen.name == "Games List") {
			Events.instance.Raise (new UpdateDrawerEvent ());
		} else {
			GotoScreen ("Games List");
		}
	}

	void OnConnectedToServerEvent (ConnectedToServerEvent e) {
		MessageSender.instance.SendMessageToHost ("RequestRegistration", playerName);
	}

	void OnHostReceiveMessageEvent (HostReceiveMessageEvent e) {
		switch (e.id) {
			case "RequestRegistration": RequestRegistration (e.message1); break;
			case "UnregisterPlayer": UnregisterPlayer (e.message1); break;
		}
	}

	void OnAllReceiveMessageEvent (AllReceiveMessageEvent e) {
		switch (e.id) {
			case "AcceptPlayer": AcceptPlayer (e.message1); break;
			case "RejectPlayer": RejectPlayer (e.message1); break;
			case "ClearPlayerList": ClearPlayerList (); break;
			case "AddPlayer": AddPlayer (e.message1); break;
			case "ListRefreshed": RaiseRefreshPlayerList (); break;
		}
	}
}
