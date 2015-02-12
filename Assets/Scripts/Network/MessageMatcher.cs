using UnityEngine;
using System.Collections;

[RequireComponent (typeof (NetworkView))]
public class MessageMatcher : MonoBehaviour {

	string id = "";
	string[] players = new string[0];
	string[] messages = new string[0];

	static public MessageMatcher instance;

	void Awake () {
		if (instance == null)
			instance = this;
		Events.instance.AddListener<RefreshPlayerListEvent> (OnRefreshPlayerListEvent);
	}

	public void SetMessage (string id, string message) {
		this.id = id;
		if (Network.isServer) {
			SetPlayerMessage (Player.instance.Name, message);
		} else {
			networkView.RPC ("SetPlayerMessage", RPCMode.Server, Player.instance.Name, message);
		}
	}

	int GetPlayerIndex (string playerName) {
		for (int i = 0; i < players.Length; i ++) {
			if (players[i] == playerName || players[i] == "")
				return i;
		}
		return -1;
	}

	bool MessagesMatch () {
		string match = messages[0];
		for (int i = 1; i < messages.Length; i ++) {
			if (messages[i] != match)
				return false;
		}
		return true;
	}

	void Clear () {
		id = "";
		for (int i = 0; i < messages.Length; i ++) {
			messages[i] = "";
		}
	}

	/**
	 *	Events
	 */

	void OnRefreshPlayerListEvent (RefreshPlayerListEvent e) {
		players = e.playerNames;
		messages = new string[players.Length];
	}

	/**
	 *	RPCs
	 */

	[RPC]
	void SetPlayerMessage (string playerName, string message) {
		messages[GetPlayerIndex (playerName)] = message;
		if (MessagesMatch ()) {
			networkView.RPC ("RaiseMessagesMatch", RPCMode.All, id, message);
		}
	}

	[RPC]
	void RaiseMessagesMatch (string id, string message) {
		Events.instance.Raise (new MessagesMatchEvent (id, message));
		Clear ();
	}

	/**
	 *	Debugging
	 */

	/*void Update () {
		if (Input.GetKeyDown (KeyCode.Q)) {
			SetMessage ("Broccoli");
		}
		if (Input.GetKeyDown (KeyCode.A)) {
			SetMessage ("Banana");
		}
		if (Input.GetKeyDown (KeyCode.Z)) {
			SetMessage ("gorped");
		}
	}*/
}
