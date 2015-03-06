using UnityEngine;
using System.Collections;

public class MessageMatcher : MonoBehaviour {

	string id = "";
	string[] players = new string[0];
	string[] messages = new string[0];

	static public MessageMatcher instance;

	void Awake () {
		if (instance == null)
			instance = this;
		Events.instance.AddListener<RefreshPlayerListEvent> (OnRefreshPlayerListEvent);
		Events.instance.AddListener<HostReceiveMessageEvent> (OnHostReceiveMessageEvent);
		Events.instance.AddListener<AllReceiveMessageEvent> (OnAllReceiveMessageEvent);
	}

	public void SetMessage (string id, string message) {
		this.id = id;
		if (MultiplayerManager2.instance.Hosting) {
			SetPlayerMessage (Player.instance.Name, message);
		} else {
			MessageSender.instance.SendMessageToHost ("SetPlayerMessage", Player.instance.Name, message);
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

	void SetPlayerMessage (string playerName, string message) {
		messages[GetPlayerIndex (playerName)] = message;
		if (MessagesMatch ()) {
			MessageSender.instance.SendMessageToAll ("RaiseMessagesMatch", id, message);
		}
	}

	void RaiseMessagesMatch (string id, string message) {
		Events.instance.Raise (new MessagesMatchEvent (id, message));
		Clear ();
	}

	/**
	 *	Events
	 */

	void OnRefreshPlayerListEvent (RefreshPlayerListEvent e) {
		players = e.playerNames;
		messages = new string[players.Length];
	}

	void OnHostReceiveMessageEvent (HostReceiveMessageEvent e) {
		if (e.id == "SetPlayerMessage") {
			SetPlayerMessage (e.message1, e.message2);
		}
	}

	void OnAllReceiveMessageEvent (AllReceiveMessageEvent e) {
		if (e.id == "RaiseMessagesMatch") {
			RaiseMessagesMatch (e.message1, e.message2);
		}
	}
}
