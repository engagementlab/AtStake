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

	bool usingWifi = false;
	bool UsingWifi {
		get {
			if (usingWifi == false) {
				usingWifi = MultiplayerManager2.instance.UsingWifi;
			}
			return usingWifi;
		}
	}

	List<string> Peers {
		get { return MultiPeer.getConnectedPeers(); }
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
		
		if (!UsingWifi) {
			SendMessageToAll (message.name, message.message1, message.message2, message.val);
			return;
		}
		
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
		
		if (!UsingWifi) {
			MultiPeer.sendMessageToAllPeers ("MessageSender", "OnMultiPeerReceiveMessage", MessageToString (id, message1, message2, val));
			Events.instance.Raise (new AllReceiveMessageEvent (id, message1, message2, val));
			return;
		}

		if (Connected) {
			networkView.RPC ("AllReceiveMessage", RPCMode.All, id, message1, message2, val);
		} else {
			AllReceiveMessage (id, message1, message2, val);
		}
	}

	public void SendMessageToHost (string id, string message1="", string message2="", int val=-1) {

		if (!UsingWifi) {
			//MultiPeer.sendMessageToPeers (new string[] { Peers[0] }, "MessageSender", "OnMultiPeerHostReceiveMessage", MessageToString (id, message1, message2, val));
			// TODO: Similar to Decider below, cache the name of the host
			MultiPeer.sendMessageToAllPeers ("MessageSender", "OnMultiPeerHostReceiveMessage", MessageToString (id, message1, message2, val));
			return;
		}

		networkView.RPC ("HostReceiveMessage", RPCMode.Server, id, message1, message2);
	}

	public void SendMessageToDecider (string id, string message1="", string message2="", int val=-1) {
		
		// TODO: When Decider is selected, set the Peer so that we can direct the message just to the Decider
		// (instead of all players hearing the message and checking if they're the Decider)
		if (!UsingWifi) {
			MultiPeer.sendMessageToAllPeers ("MessageSender", "OnMultiPeerDeciderReceiveMessage", MessageToString (id, message1, message2, val));
			return;
		}

		networkView.RPC ("DeciderReceiveMessage", RPCMode.All, id, message1, message2);
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

	// Specific to Bluetooth: MultiPeer only passes strings, so the message needs to be
	// combined into a string delimited by '|', and then split once it's received
	string MessageToString (string id, string message1, string message2, int val) {
		return string.Format ("{0}|{1}|{2}|{3}", id, message1, message2, val);
	}

	NetworkMessage StringToMessage (string str) {
		string[] parts = str.Split ('|');
		string id = parts[0];
		string message1 = parts[1];
		string message2 = parts[2];
		int val = int.Parse (parts[3]);
		return new NetworkMessage (id, message1, message2, val);
	}

	/**
	 *	Messages
	 */

	void OnRefreshPlayerListEvent (RefreshPlayerListEvent e) {
		clientCount = e.playerNames.Length-1; // subtract 1 because we don't include the host
	}

	void OnMultiPeerReceiveMessage (string param) {
		NetworkMessage message = StringToMessage (param);
		Events.instance.Raise (new AllReceiveMessageEvent (message.name, message.message1, message.message2, message.val));
	}

	void OnMultiPeerHostReceiveMessage (string param) {
		if (MultiplayerManager2.instance.Hosting) {
			NetworkMessage message = StringToMessage (param);
			Debug.Log ("raise host receive message " + param);
			Events.instance.Raise (new HostReceiveMessageEvent (message.name, message.message1, message.message2));
		}
	}

	void OnMultiPeerDeciderReceiveMessage (string param) {
		if (Player.instance.IsDecider) {
			NetworkMessage message = StringToMessage (param);
			Events.instance.Raise (new DeciderReceiveMessageEvent (message.name, message.message1, message.message2));
		}
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

	[RPC]
	void HostReceiveMessage (string id, string message1, string message2) {
		Events.instance.Raise (new HostReceiveMessageEvent (id, message1, message2));
	}

	[RPC]
	void DeciderReceiveMessage (string id, string message1, string message2, int val) {
		if (Player.instance.IsDecider) {
			Events.instance.Raise (new DeciderReceiveMessageEvent (id, message1, message2));
		}
	}
}
