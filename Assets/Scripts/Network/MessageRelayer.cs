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
	}

	/**
	 *	Host functions
	 */

	public void ScheduleMessage (string message) {
		messages.Add (message);
		if (messages.Count == 1) {
			HostSendMessage ();
			messages.RemoveAt (0);
		}
	}

	/**
	 *	Client functions
	 */

	public void ConfirmMessageReceived () {
		networkView.RPC ("ConfirmHostMessageReceived", RPCMode.Server);
	}

	/**
	 *	Private functions
	 */

	void HostReceiveConfirmation () {
		
		if (messages.Count == 0)
			return;

		receivedCount ++;
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

	[RPC]
	void ConfirmHostMessageReceived () {

		// The host hears this when a client confirms that it's received the message
		HostReceiveConfirmation ();
	}
}

