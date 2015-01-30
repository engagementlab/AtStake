using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BeanPoolManager : MonoBehaviour {

	public static BeanPoolManager instance;
	int playerCount = 0;
	int scoreUpdateCount = 0;

	List<string> names = new List<string>();
	List<int> scores = new List<int>();

	void Awake () {
		if (instance == null)
			instance = this;

		Events.instance.AddListener<AllReceiveMessageEvent> (OnAllReceiveMessageEvent);
		Events.instance.AddListener<RefreshPlayerListEvent> (OnRefreshPlayerListEvent);
		Events.instance.AddListener<GameEndEvent> (OnGameEndEvent);
	}

	public void UpdateMyScore () {
		Player player = Player.instance;
		MessageSender.instance.SendMessageToAll ("UpdatePlayerScore", player.Name, "", player.MyBeanPool.BeanCount);
	}

	void UpdatePlayerScore (string playerName, int playerScore) {
		int index = GetNameIndex (playerName);
		if (index == -1) {
			names.Add (playerName);
			scores.Add (playerScore);
		} else {
			names[index] = playerName;
			scores[index] = playerScore;
		}
		scoreUpdateCount ++;
		if (scoreUpdateCount == playerCount) {
			scoreUpdateCount = 0;
			SortPlayers ();
		}
	}

	int GetNameIndex (string name) {
		for (int i = 0; i < names.Count; i ++) {
			if (names[i] == name)
				return i;
		}
		return -1;
	}

	void OnRefreshPlayerListEvent (RefreshPlayerListEvent e) {
		playerCount = e.playerNames.Length;
	}

	void OnAllReceiveMessageEvent (AllReceiveMessageEvent e) {
		if (e.id == "UpdatePlayerScore") {
			UpdatePlayerScore (e.message1, e.val);
		}
	}

	void SortPlayers () {

		List<string> newPlayerNames = new List<string> ();
		List<int> newPlayerScores = new List<int>();

		for (int i = 0; i < scores.Count; i ++) {
			int score = scores[i];
			string name = names[i];
			if (i == 0) {
				newPlayerScores.Add (score);
				newPlayerNames.Add (name);
				continue;
			}
			for (int j = 0; j < newPlayerScores.Count; j ++) {
				if (score > newPlayerScores[j]) {
					newPlayerScores.Insert (j, score);
					newPlayerNames.Insert (j, name);
					break;
				} else if (j == newPlayerScores.Count-1) {
					newPlayerScores.Add (score);
					newPlayerNames.Add (name);
					break;
				}
			}
		}

		names = newPlayerNames;
		scores = newPlayerScores;

		string[] namesArr = new string[names.Count];
		int[] scoresArr = new int[scores.Count];
		for (int i = 0; i < namesArr.Length; i ++) {
			namesArr[i] = names[i];
			scoresArr[i] = scores[i];
		}

		// This should be its own function, but... eghhh
		Events.instance.Raise (new UpdatedPlayerScoresEvent (namesArr, scoresArr));
	}

	void OnGameEndEvent (GameEndEvent e) {
		names.Clear ();
		scores.Clear ();
	}
}
