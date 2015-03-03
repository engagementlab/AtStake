using UnityEngine;
using System.Collections;

// new network manager
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
