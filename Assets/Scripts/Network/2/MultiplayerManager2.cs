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

	void Awake () {
		
		if (instance == null)
			instance = this;

		networkingManager = Instantiate (networkingManager) as NetworkingManager;
		Hosting = false;
		
		Events.instance.AddListener<EnterNameEvent> (OnEnterNameEvent);
		Events.instance.AddListener<FoundGamesEvent> (OnFoundGamesEvent);
		Events.instance.AddListener<ConnectedToServerEvent> (OnConnectedToServerEvent);
	}

	void GotoScreen (string screenName) {
		GameStateController.instance.GotoScreen (screenName);
	}

	/**
	 *	Hosting
	 */

	public void HostGame () {
		Hosting = true;
		networkingManager.HostGame (playerName);
		playerList.Init (playerName);
	}

	/**
	 *	Joining
	 */

	public void JoinGame () {
		networkingManager.JoinGame ();
	}

	public void ConnectToHost (HostData hostAttempt) {
		Hosting = false;
		this.hostAttempt = hostAttempt;
		networkingManager.ConnectToHost (hostAttempt);
	}

	/**
	 *	Events
	 */

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
		networkView.RPC ("RequestRegistration", RPCMode.Server, playerName);
	}

	/**
	 *	RPCs
	 */

	[RPC] void RequestRegistration (string clientName) {
		Debug.Log (clientName);
	}
}
