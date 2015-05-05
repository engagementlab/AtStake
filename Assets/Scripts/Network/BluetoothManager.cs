using UnityEngine;
using System.Collections;

public class BluetoothManager : MonoBehaviour {

	readonly string gameName = "atstake";
	bool hosting = false;

	void Awake () {
		MultiPeerManager.peerDidChangeStateToConnectedEvent += peerDidChangeStateToConnectedEvent;
		Events.instance.AddListener<AllReceiveMessageEvent> (OnAllReceiveMessageEvent);
	}

	/**
	 *	Hosting
	 */

	public void HostGame (string playerName) {
		hosting = true;
		AdvertiseDevice ();
		MultiPeer.showPeerPicker ();
	}

	public void DisconnectHost () {
		MessageSender.instance.SendMessageToAll ("OnDisconnectedFromServer");
		MultiPeer.disconnectAndEndSession ();
	}

	public void InviteMore () {
		MultiPeer.showPeerPicker ();
	}

	/**
	 *	Joining
	 */

	public void JoinGame () {
		hosting = false;
		AdvertiseDevice ();
	}

	public void DisconnectFromHost () {
		MultiPeer.disconnectAndEndSession ();
	}
	
	void AdvertiseDevice () {
		MultiPeer.advertiseCurrentDevice (true, gameName, Player.instance.Name);
	}

	void peerDidChangeStateToConnectedEvent (string param) {
		if (!hosting) {
			Events.instance.Raise (new ConnectedToServerEvent ());
		} 
	}

	/**
	 *	Events
	 */

	void OnAllReceiveMessageEvent (AllReceiveMessageEvent e) {
		if (e.id == "OnDisconnectedFromServer") {
			OnDisconnectedFromServer ();
		}
	}

	void OnDisconnectedFromServer () {
		DisconnectFromHost ();
		Events.instance.Raise (new DisconnectedFromServerEvent (hosting));		
		hosting = false;
	}
}
