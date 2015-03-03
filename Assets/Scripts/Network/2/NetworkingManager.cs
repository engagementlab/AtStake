using UnityEngine;
using System.Collections;

// new network manager
// acts as a dispatcher between ServerManager and BluetoothManager:
// if the server's up, use the ServerManager; otherwise, use the BluetoothManager
public class NetworkingManager : MonoBehaviour {

	public ServerManager serverManager;
	public BluetoothManager bluetoothManager;

	ConnectionType connectionType = ConnectionType.None;
	enum ConnectionType {
		None,
		Wifi,
		Bluetooth
	}

	bool Wifi { get { return connectionType == ConnectionType.Wifi; } }
	bool Bluetooth { get 
		{ 
			return connectionType == ConnectionType.Bluetooth || 
				   connectionType == ConnectionType.None; 
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
			//bluetoothManager.HostGame (playerName):
		}
	}

	public void DisconnectHost () {
		if (Wifi) {
			serverManager.DisconnectHost ();
		}
	}

	/**
	 *	Joining
	 */

	public void JoinGame () {
		if (Wifi) {
			serverManager.JoinGame ();
		} else if (Bluetooth) {
			//bluetoothManager.JoinGame ();
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
