using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class NetworkMessage {

	public readonly string name, message1, message2;
	public readonly int val;

	public NetworkMessage (string name, string message1="", string message2="", int val=-1) {
		this.name = name;
		this.message1 = message1;
		this.message2 = message2;
		this.val = val;
	}
}

[RequireComponent (typeof (NetworkView))]
public class MessageSender : MonoBehaviour {

	List<NetworkMessage> messages = new List<NetworkMessage>();
	int receivedCount = 0;
	int clientCount = 0;

	bool AllReceived {
		get { return receivedCount >= clientCount; }
	}

	NetworkMessage CurrentMessage {
		get { return messages[0]; }
	}

	bool Connected {
		get { return Network.isClient || Network.isServer; }
	}

	static public MessageSender instance;

	void Awake () {
		if (instance == null)
			instance = this;

		Events.instance.AddListener<RefreshPlayerListEvent> (OnRefreshPlayerListEvent);
	}

	/**
	 *	Public functions
	 */

	public void ScheduleMessage (string name) {
		ScheduleMessage (new NetworkMessage (name));
	}

	public void ScheduleMessage (NetworkMessage message) {
		if (Network.isServer) {
			AddMessage (message);
		} else {
			if (Connected) {
				networkView.RPC ("HostAddMessage", RPCMode.Server, message.name, message.message1, message.message2, message.val);
			} else {
				messages = new List<NetworkMessage> (new NetworkMessage[] {message});
				HostSendMessage ();
			}
		}
	}

	public void SendMessageToAll (string id, string message1="", string message2="", int val=-1) {
		if (Connected) {
			networkView.RPC ("AllReceiveMessage", RPCMode.All, id, message1, message2, val);
		} else {
			AllReceiveMessage (id, message1, message2, val);
		}
	}

	/**
	 *	Private functions
	 */

	void HostReceiveConfirmation (string name) {
		if (messages.Count > 0) {
			receivedCount ++;
			RemoveMessage ();
		}
	}

	void AddMessage (NetworkMessage message) {
		messages.Add (message);
		if (messages.Count == 1) {
			HostSendMessage ();
		}
	}

	void RemoveMessage () {
		if (AllReceived) {
			messages.RemoveAt (0);
			receivedCount = 0;
			if (messages.Count > 0) {
				HostSendMessage ();
			}
		}
	}

	void HostSendMessage () {
		Events.instance.Raise (new HostSendMessageEvent (CurrentMessage.name, CurrentMessage.message1, CurrentMessage.message2, CurrentMessage.val));
		if (Connected) networkView.RPC ("RequestClientConfirmation", RPCMode.Others, CurrentMessage.name);
	}

	/**
	 *	Messages
	 */

	void OnRefreshPlayerListEvent (RefreshPlayerListEvent e) {
		clientCount = e.playerNames.Length-1; // subtract 1 because we don't include the host
	}

	/**
	 *	RPCs
	 */

	[RPC]
	void HostAddMessage (string name, string message1, string message2, int val) {
		AddMessage (new NetworkMessage (name, message1, message2, val));
	}

	[RPC]
	void RequestClientConfirmation (string name) {
		networkView.RPC ("ConfirmClientReceived", RPCMode.Server, name);
	}

	[RPC]
	void ConfirmClientReceived (string name) {
		HostReceiveConfirmation (name);
	}

	[RPC]
	void AllReceiveMessage (string id, string message1, string message2, int val) {
		Events.instance.Raise (new AllReceiveMessageEvent (id, message1, message2, val));
	}
}
