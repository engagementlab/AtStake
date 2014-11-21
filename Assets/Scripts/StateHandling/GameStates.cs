using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameStates {

	List<GameState> states = new List<GameState> ();

	public GameStates () {
		states.Add (new StartState ());
		states.Add (new MultiplayerState ());
		states.Add (new DeciderState ());
		states.Add (new RoundState ());
		states.Add (new EndState ());
	}

	public GameState FirstState () {
		return GetState (0);
	}

	public GameState LastState () {
		return GetState (StatesCount () - 1);
	}

	public GameState GetState (int index) {
		return states[index];
	}
	
	public int GetStateIndex (string name) {
		for (int i = 0; i < StatesCount (); i ++) {
			if (states[i].name == name)
				return i;
		}
		Debug.LogError ("Couldn't find state named " + name);
		return -1;
	}

	public int StatesCount () {
		return states.Count;
	}
}
