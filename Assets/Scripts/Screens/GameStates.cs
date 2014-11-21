using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameStates {

	List<GameState> states = new List<GameState> ();

	public GameStates () {
		AddState ("Start Screen", new string[] { "Play", "Instructions", "New Deck", "About" });
		AddState ("Multiplayer", new string[] { "Enter Name", "Host or Join", "Games List", "Lobby" });
		AddState ("Choose Decider", new string[] { "Approval", "Choose Deck" });
		AddState ("Round", new string[] { "Brainstorm", "Pitch", "Deliberate", "Decide", "Agenda Bonuses", "Scoreboard" });
		AddState ("End Screen", new string[] { "Scoreboard" });
	}

	public void AddState (string name, string[] screenNames) {

		int screenCount = screenNames.Length;
		GameScreen[] screens = new GameScreen[screenCount];

		for (int i = 0; i < screens.Length; i ++) {
			screens[i] = new GameScreen (screenNames[i]);
		}

		states.Add (new GameState (
			name,
			screens
		));
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
