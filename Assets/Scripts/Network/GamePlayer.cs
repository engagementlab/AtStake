using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GamePlayer {

	public string playerName;
	List<string> players = new List<string>();

	public int PlayerCount {
		get { return players.Count; }
	}

	public List<string> Players {
		get { return players; }
	}

	public GamePlayer (string playerName) {
		this.playerName = playerName;
		Events.instance.AddListener<GameEndEvent> (OnGameEndEvent);
	}

	public void AddPlayer (string name) {
		players.Add (name);
	}

	public void RemovePlayer (string name) {
		players.Remove (name);
	}

	public bool HasPlayers () {
		return players.Count > 0;
	}

	public bool HasOtherPlayers () {
		return players.Count > 1;
	}

	public string[] GetPlayerNames () {
		string[] names = new string[players.Count];
		int count = 0;
		players.ForEach (delegate (string name) {
			names[count] = name;
			count ++;
		});
		return names;
	}

	public void ClearPlayers () {
		players.Clear ();
	}

	public void OnGameEndEvent (GameEndEvent e) {
		ClearPlayers ();
	}
}
