using UnityEngine;
using System.Collections;

public class NetworkManager : MonoBehaviour {

	readonly string gameName = "@Stake";

	int maxConnections = 6;
	bool secureServer = false;
	HostData[] hosts = new HostData[0];

	bool hosting = false;
	bool connected = false;

	string testMessage = "";

	void Awake () {
		MasterServer.ClearHostList ();
	}

	void OnGUI () {
		if (GUILayout.Button ("Host game")) {
			InitHost ();
		}
		if (GUILayout.Button ("Find games")) {
			RefreshHostList ();
		}
		if (hosts.Length > 0) {
			for (int i = 0; i < hosts.Length; i ++) {
				if (GUILayout.Button (hosts[i].gameName)) {
					ConnectToHost (hosts[i]);
				}
			}
		}
		if (connected) {
			if (GUILayout.Button ("Send test message")) {
				networkView.RPC ("TestMessaging", RPCMode.All, "freaking hi");
			}
		}
		GUILayout.Label (testMessage);
	}

	// Hosting

	void InitHost () {
		StartServer ("test");
	}

	void StartServer (string gameInstanceName) {
		if (secureServer)
			Network.InitializeSecurity ();

		// Use NAT punchthrough if no public IP present
		Network.InitializeServer (maxConnections, 25001, !Network.HavePublicAddress ());
		MasterServer.RegisterHost (gameName, gameInstanceName);
		hosting = true;
	}

	// Joining

	void RefreshHostList () {
		MasterServer.RequestHostList (gameName);
		StartCoroutine (FindHosts ());
	}

	IEnumerator FindHosts () {

		// time in seconds before we give up on finding a new game
		float timeout = 20f; 

		while (hosts.Length == 0 && timeout > 0f) {
			hosts = MasterServer.PollHostList ();
			timeout -= Time.deltaTime;
			yield return null;
		}

		MasterServer.ClearHostList ();
		if (timeout <= 0f) {
			OnTimeout ();
		}
	}

	void OnTimeout () {
		Debug.Log ("couldn't find a single freakin host!!");
	}

	void ConnectToHost (HostData host) {
		Network.Connect (host);
	}

	void OnConnectedToServer () {
		Debug.Log ("connected to host");
		connected = true;
	}

	// Testing

	[RPC]
	void TestMessaging (string message) {
//		Debug.Log (message);
		testMessage = message;
	}
}
