using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// new multiplayer manager
public class PlayerList {

	List<string> players = new List<string> ();
	public List<string> Players {
		get { return players; }
	}

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
		if (players.Contains (name))
			return false;
		players.Add (name);
		return true;
	}

	public bool Has (string name) {
		return players.Contains (name);
	}

	public void Remove (string name) {
		players.Remove (name);
	}

	public void Clear () {
		players.Clear ();
	}
}