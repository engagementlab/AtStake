using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// new multiplayer manager
[RequireComponent (typeof(NetworkView))]
public class MultiplayerManager2 : MonoBehaviour {

	public static MultiplayerManager2 instance;
	public NetworkManager networkManager;

	string playerName = "";
	string PlayerName {
		get {
			if (playerName == "" || playerName != Player.instance.Name) {
				playerName = Player.instance.Name;
			}
			return playerName;
		}
	}

	public bool Hosting { get; private set; }

	PlayerList playerList = new PlayerList ();

	void Awake () {
		if (instance == null)
			instance = this;
	}

	void Start () {
		networkManager = Instantiate (networkManager) as NetworkManager;
	}

	/**
	 *	Host
	 */

	public void HostEnter () {
		networkManager.HostGame (PlayerName);
		playerList.Add (PlayerName);
		Hosting = true;
	}

	void HostExit () {
		networkManager.StopServer ();
		playerList.Clear ();
		Hosting = false;
	}

	/**
	 *	Player (joining)
	 */

	public void PlayerEnter () {

	}

	void PlayerExit () {

	}

	public void ExitLobby () {
		if (Hosting) {
			HostExit ();
		} else {
			PlayerExit ();
		}
	}
}
