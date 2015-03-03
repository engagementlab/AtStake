using UnityEngine;
using System.Collections;

public class BluetoothManager : MonoBehaviour {

	readonly string gameName = "atstake";
	bool hosting = false;

	void Awake () {
		MultiPeerManager.peerDidChangeStateToConnectedEvent += peerDidChangeStateToConnectedEvent;
	}

	/**
	 *	Hosting
	 */

	public void HostGame (string playerName) {
		hosting = true;
		AdvertiseDevice ();
		MultiPeer.showPeerPicker();
	}

	/**
	 *	Joining
	 */

	public void JoinGame () {
		hosting = false;
		AdvertiseDevice ();
	}

	public void DisconnectFromHost () {
		Debug.Log ("disconnect");
		MultiPeer.disconnectAndEndSession();
	}

	void AdvertiseDevice () {
		MultiPeer.advertiseCurrentDevice (true, gameName);
	}

	void peerDidChangeStateToConnectedEvent (string param) {
		// both host and client hear this
		Debug.Log ("connected to server");
		Events.instance.Raise (new ConnectedToServerEvent ());
	}
}
