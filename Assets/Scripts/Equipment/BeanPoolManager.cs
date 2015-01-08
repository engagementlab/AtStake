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

		Events.instance.AddListener<AllReceiveMessageEvent> (OnAllReceiveMessageEvent);
		Events.instance.AddListener<RefreshPlayerListEvent> (OnRefreshPlayerListEvent);

		DebugSortPlayers ();
	}

	public void UpdateMyScore () {
		Player player = Player.instance;
		MessageSender.instance.SendMessageToAll ("UpdatePlayerScore", player.Name, "", player.MyBeanPool.BeanCount);
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

	void OnAllReceiveMessageEvent (AllReceiveMessageEvent e) {
		if (e.id == "UpdatePlayerScore") {
			UpdatePlayerScore (e.message1, e.val);
		}
	}

	void SortPlayers () {
		
		// this doesn't work on ios
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
		}

		// This should be its own function, but... eghhh
		Events.instance.Raise (new UpdatedPlayerScoresEvent (names, scores));
	}

	/**
	 *	Debugging
	 */

	// TODO: Replace SortPlayers() with this
	void DebugSortPlayers () {
		
		List<string> playerNames = new List<string> ();
		playerNames.Add ("forrest");
		playerNames.Add ("dan");
		playerNames.Add ("jenny");
		playerNames.Add ("stephanie");
		
		List<int> playerScores = new List<int> ();
		playerScores.Add (6);
		playerScores.Add (8);
		playerScores.Add (5);
		playerScores.Add (4);

		List<string> newPlayerNames = new List<string> ();
		List<int> newPlayerScores = new List<int>();

		for (int i = 0; i < playerScores.Count; i ++) {
			int playerScore = playerScores[i];
			string playerName = playerNames[i];
			if (i == 0) {
				newPlayerScores.Add (playerScore);
				newPlayerNames.Add (playerName);
				continue;
			}
			for (int j = 0; j < newPlayerScores.Count; j ++) {
				if (playerScore > newPlayerScores[j]) {
					newPlayerScores.Insert (j, playerScore);
					newPlayerNames.Insert (j, playerName);
					break;
				} else if (j == newPlayerScores.Count-1) {
					newPlayerScores.Add (playerScore);
					newPlayerNames.Add (playerName);
					break;
				}
			}
		}
		PrintScores (newPlayerNames, newPlayerScores);
	}

	void PrintScores (List<string> playerNames, List<int> playerScores) {
		Debug.Log("--------");
		for (int i = 0; i < playerScores.Count; i ++) {
			Debug.Log (playerNames[i] + ": " + playerScores[i]);
		}
	}
}
