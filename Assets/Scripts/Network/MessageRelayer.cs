using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// Deprecate this! Use this MessageSender instead

[RequireComponent (typeof (NetworkView))]
public class MessageRelayer : MonoBehaviour {

	List<string> messages = new List<string>();
	
	int receivedCount = 0;
	int clientCount = 0;

	bool AllReceived {
		get { return receivedCount >= clientCount; }
	}

	bool Connected {
		get { return Network.isClient || Network.isServer; }
	}

	static public MessageRelayer instance;

	/**
	 *	1. Host sends a message to everyone
	 *  2. Clients each receive the message, and send confirmation to 
	 *		the host that they heard it
	 *	3. Once the host sees that all clients have heard the message, 
	 *		it sends the next message (if one has been queued)
	 */

	void Awake () {

		if (instance == null)
			instance = this;

		Events.instance.AddListener<RefreshPlayerListEvent> (OnRefreshPlayerListEvent);
		Events.instance.AddListener<HostScheduleMessageEvent> (OnHostScheduleMessageEvent);
		Events.instance.AddListener<ClientConfirmMessageEvent> (OnClientConfirmMessageEvent);
	}

	/**
	 *	Host functions
	 */

	public void ScheduleMessage (string message) {
		messages.Add (message);
		if (messages.Count == 1) {
			HostSendMessage ();
		}
	}

	// Send to everyone except the Decider
	public void SendMessageToPlayers (string message1, string message2="") {
		if (Connected) networkView.RPC ("PlayersReceiveMessage", RPCMode.All, message1, message2);
	}

	// Send to the Decider
	public void SendMessageToDecider (string id, string message1="", string message2="") {
		if (Connected) networkView.RPC ("DeciderReceiveMessage", RPCMode.All, id, message1, message2);
	}

	// Send to a specific player
	public void SendMessageToPlayer (string playerName, string message="") {
		if (Connected) networkView.RPC ("PlayerReceiveMessage", RPCMode.All, playerName, message);
	}

	// Send to everyone except the Decider and a specific player
	public void SendMessageToOthers (string playerName, string message="") {
		if (Connected) networkView.RPC ("OthersReceiveMessage", RPCMode.All, playerName, message);
	}

	public void SendMessageToAll (string id, string message1="", string message2="") {
		if (Connected) {
			networkView.RPC ("AllReceiveMessage", RPCMode.All, id, message1, message2);
		} else {
			AllReceiveMessage (id, message1, message2);
		}
	}

	public void SendMessageToHost (string id, string message1="", string message2="") {
		if (Connected) networkView.RPC ("HostReceiveMessage", RPCMode.Server, id, message1, message2);
	}

	/**
	 *	Client functions
	 */

	public void ConfirmMessageReceived (string message) {
		if (Connected) networkView.RPC ("ConfirmHostMessageReceived", RPCMode.Server, message);
	}

	/**
	 *	Private functions
	 */

	void HostReceiveConfirmation (string message) {
		
		// ignore if there are no messages to send
		if (messages.Count == 0 || message != messages[0])
			return;

		receivedCount ++;

		// if all clients have confirmed, send the next message
		if (AllReceived) {
			messages.RemoveAt (0);
			receivedCount = 0;
			if (messages.Count > 0) {
				HostSendMessage ();
			}
		}
	}

	void HostSendMessage () {
		Events.instance.Raise (new HostSendMessageEvent (messages[0]));
	}

	/**
	 *	Events
	 */

	void OnRefreshPlayerListEvent (RefreshPlayerListEvent e) {
		clientCount = e.playerNames.Length-1; // -1 because we don't include the host
	}

	void OnHostScheduleMessageEvent (HostScheduleMessageEvent e) {
		if (MultiplayerManager.instance.Hosting) {
			ScheduleMessage (e.message);
		} else {
			networkView.RPC ("HostScheduleMessage", RPCMode.Server, e.message);
		}
	}

	void OnClientConfirmMessageEvent (ClientConfirmMessageEvent e) {
		if (!MultiplayerManager.instance.Hosting) {
			ConfirmMessageReceived (e.message);
		}
	}

	[RPC]
	void HostScheduleMessage (string message) {

		// This is used when a client wants the host to schedule a message
		ScheduleMessage (message);
	}

	[RPC]
	void ConfirmHostMessageReceived (string message) {

		// The host hears this when a client confirms that it's received the message
		HostReceiveConfirmation (message);
	}

	[RPC]
	void PlayerReceiveMessage (string playerName, string message) {

		// Only the player with the name playerName will hear this message
		if (playerName == Player.instance.Name) {
			Events.instance.Raise (new PlayerReceiveMessageEvent (message));
		}
	}

	[RPC]
	void OthersReceiveMessage (string playerName, string message) {

		// Every player except the Decider and the one named playerName will hear this message
		if (playerName != Player.instance.Name && !Player.instance.IsDecider) {
			Events.instance.Raise (new OthersReceiveMessageEvent (message));
		}
	}

	[RPC]
	void PlayersReceiveMessage (string message1, string message2) {

		// All players except the Decider will hear this message
		if (!Player.instance.IsDecider) {
			Events.instance.Raise (new PlayersReceiveMessageEvent (message1, message2));
		}
	}

	[RPC]
	void DeciderReceiveMessage (string id, string message1, string message2) {

		// Only the Decider will hear this message
		if (Player.instance.IsDecider) {
			Events.instance.Raise (new DeciderReceiveMessageEvent (id, message1, message2));
		}
	}

	[RPC]
	void AllReceiveMessage (string id, string message1, string message2) {
		Events.instance.Raise (new AllReceiveMessageEvent (id, message1, message2));
	}

	[RPC]
	void HostReceiveMessage (string id, string message1, string message2) {
		Events.instance.Raise (new HostReceiveMessageEvent (id, message1, message2));
	}
}

