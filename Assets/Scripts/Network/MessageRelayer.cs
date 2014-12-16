using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent (typeof (NetworkView))]
public class MessageRelayer : MonoBehaviour {

	List<string> messages = new List<string>();
	
	int receivedCount = 0;
	int clientCount = 0;

	bool AllReceived {
		get { return receivedCount >= clientCount; }
	}

	/**
	 *	1. Host sends a message to everyone
	 *  2. Clients each receive the message, and send confirmation to 
	 *		the host that they heard it
	 *	3. Once the host sees that all clients have heard the message, 
	 *		it sends the next message (if one has been queued)
	 */

	void Awake () {
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

	/**
	 *	Client functions
	 */

	public void ConfirmMessageReceived (string message) {
		networkView.RPC ("ConfirmHostMessageReceived", RPCMode.Server, message);
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
		ScheduleMessage (e.message);
	}

	void OnClientConfirmMessageEvent (ClientConfirmMessageEvent e) {
		ConfirmMessageReceived (e.message);
	}

	[RPC]
	void ConfirmHostMessageReceived (string message) {

		// The host hears this when a client confirms that it's received the message
		HostReceiveConfirmation (message);
	}
}

