using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class BeanPoolManager : MonoBehaviour {

	public static BeanPoolManager instance;
	int playerCount = 0;
	Dictionary<string, int> playerScores = new Dictionary<string, int>();
	int scoreUpdateCount = 0;

	void Awake () {
		if (instance == null)
			instance = this;

		Events.instance.AddListener<AllReceiveMessageEvent> (OnAllReceiveMessage);
		Events.instance.AddListener<RefreshPlayerListEvent> (OnRefreshPlayerListEvent);
	}

	// Debugging
	/*void Update () {
		if (Input.GetKeyDown (KeyCode.Space)) {
			Events.instance.Raise (new RefreshPlayerListEvent (new string[] { "Forrest", "Jenny", "Dan", "Mama" }));
			MessageRelayer.instance.SendMessageToAll ("UpdatePlayerScore", "Forrest", "9");
			MessageRelayer.instance.SendMessageToAll ("UpdatePlayerScore", "Jenny", "2");
			MessageRelayer.instance.SendMessageToAll ("UpdatePlayerScore", "Dan", "1");
			MessageRelayer.instance.SendMessageToAll ("UpdatePlayerScore", "Mama", "6");
			MessageRelayer.instance.SendMessageToAll ("FinishUpdatingPlayerScores");
		}
	}*/

	public void UpdateMyScore () {
		Player player = Player.instance;
		MessageRelayer.instance.SendMessageToAll ("UpdatePlayerScore", player.Name, player.MyBeanPool.BeanCount.ToString ());
	}

	void UpdatePlayerScore (string playerName, int score) {
		if (playerScores.ContainsKey (playerName)) {
			playerScores[playerName] = score;
			scoreUpdateCount ++;
			if (scoreUpdateCount == playerScores.Count) {
				scoreUpdateCount = 0;
				SortPlayers ();
			}
		}
	}

	void OnRefreshPlayerListEvent (RefreshPlayerListEvent e) {
		playerCount = e.playerNames.Length;
		for (int i = 0; i < playerCount; i ++) {
			string playerName = e.playerNames[i];
			if (!playerScores.ContainsKey (playerName))
				playerScores.Add (playerName, 0);
		}
	}

	void OnAllReceiveMessage (AllReceiveMessageEvent e) {
		if (e.id == "UpdatePlayerScore") {
			UpdatePlayerScore (e.message1, int.Parse (e.message2));
		}
		/*if (e.id == "FinishUpdatingPlayerScores") {
			SortPlayers ();
		}*/
	}

	void SortPlayers () {
		
		var items = from pair in playerScores
		    orderby pair.Value descending
		    select pair;

		string[] names = new string[playerScores.Count];
		int[] scores = new int[playerScores.Count];
		int index = 0;
		foreach (KeyValuePair<string, int> pair in items) {
			names[index] = pair.Key;
			scores[index] = pair.Value;
			index ++;
			//Debug.Log (string.Format ("{0}: {1}", pair.Key, pair.Value));
		}

		// This should be its own function, but... eghhh
		Events.instance.Raise (new UpdatedPlayerScoresEvent (names, scores));
	}
}
