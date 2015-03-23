using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// new multiplayer manager
public class PlayerList {

	List<string> players = new List<string> ();
	public List<string> Players {
		get { return players; }
	}

	// Just used for checking if names exist in the above list
	List<string> lowerPlayers = new List<string> ();

	public string[] Names {
		get { return players.ToArray (); }
	}

	public int Count {
		get { return players.Count; }
	}

	public void Init (string name) {
		Clear ();
		Add (name);
	}

	public bool Add (string name) {
		if (lowerPlayers.Contains (name))
			return false;
		players.Add (name);
		lowerPlayers.Add (name.ToLower ());
		return true;
	}

	public bool Has (string name) {
		return players.Contains (name);
	}

	public void Remove (string name) {
		players.Remove (name);
		lowerPlayers.Remove (name.ToLower ());
	}

	public void Clear () {
		players.Clear ();
	}
}