using UnityEngine;
using System.Collections;

public class NetworkingManager : MonoBehaviour {

	public ServerManager serverManager;
	public BluetoothManager bluetoothManager;

	ConnectionType connectionType = ConnectionType.None;
	enum ConnectionType {
		None,
		Wifi,
		Bluetooth
	}

	public bool Wifi { get { return connectionType == ConnectionType.Wifi; } }
	public bool Bluetooth { 
		get {
			// If the game is still trying to determine the connection type,
			// default to Bluetooth
			if (connectionType == ConnectionType.None) {
				connectionType = ConnectionType.Bluetooth;
			} 
			return connectionType == ConnectionType.Bluetooth;
		} 
	}

	void Awake () {
		Events.instance.AddListener<ServerAvailableEvent> (OnServerAvailableEvent);
		Events.instance.AddListener<ServerUnavailableEvent> (OnServerUnavailableEvent);
	}

	/**
	 *	Hosting
	 */

	public void HostGame (string playerName) {
		if (Wifi) {
			serverManager.HostGame (playerName);
		} else if (Bluetooth) {
			bluetoothManager.HostGame (playerName);
		}
	}

	public void DisconnectHost () {
		if (Wifi) {
			serverManager.DisconnectHost ();
		} else {
			bluetoothManager.DisconnectHost ();
		}
	}

	public void InviteMore () {
		if (Bluetooth) {
			bluetoothManager.InviteMore ();
		}
	}

	/**
	 *	Joining
	 */

	public void JoinGame () {
		if (Wifi) {
			serverManager.JoinGame ();
		} else if (Bluetooth) {
			bluetoothManager.JoinGame ();
		}
	}

	public void ConnectToHost (HostData hostData) {
		if (Wifi) {
			serverManager.ConnectToHost (hostData);
		}
	}

	public void DisconnectFromHost () {
		if (Wifi) {
			serverManager.DisconnectFromHost ();
		} else {
			bluetoothManager.DisconnectFromHost ();
		}
	}

	public void StartGame () {
		if (Wifi) {
			serverManager.StartGame ();
		}
	}

	/**
	 *	Events
	 */

	void OnServerAvailableEvent (ServerAvailableEvent e) {
		connectionType = ConnectionType.Wifi;
	}

	void OnServerUnavailableEvent (ServerUnavailableEvent e) {
		connectionType = ConnectionType.Bluetooth;
	}
}
