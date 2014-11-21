using UnityEngine;
using System.Collections;

public class GameStateController : MonoBehaviour {

	GameStates states;
	int stateIndex = 0;

	// States represent categories of screens
	GameState state;
	public GameState State {
		get { return state; }
	}

	// Screens exist within states
	public GameScreen Screen {
		get { return State.Screen; }
	}

	void Awake () {
		states = new GameStates ();
		state = states.FirstState ();
	}

	public void GotoScreen (string screenName, string stateName = "") {
		if (stateName != "")
			GotoState (stateName);
		state.GotoScreen (screenName);
	}

	public void GotoState (string name) {
		stateIndex = states.GetStateIndex (name);
		state = states.GetState (stateIndex);
	}

	public void GotoNextScreen () {
		if (!state.GotoNextScreen ())
			GotoNextState ();
	}

	public void GotoNextState () {
		if (stateIndex + 1 > states.StatesCount () - 1) {
			stateIndex = 0;
		} else {
			stateIndex ++;
		}
		state = states.GetState (stateIndex);
		state.GotoFirstScreen ();
	}

	public void GotoPreviousScreen () {
		if (!state.GotoPreviousScreen ())
			GotoPreviousState ();
	}

	public void GotoPreviousState () {
		if (stateIndex == 0) {
			stateIndex = states.StatesCount () - 1;
		} else {
			stateIndex --;
		}
		state = states.GetState (stateIndex);
		state.GotoLastScreen ();
	}

	void Update () {
		// Debugging
		if (Input.GetKeyDown (KeyCode.Space)) {
			//GotoNextScreen ();
			//GotoState ("Round");
			//GotoScreen ("Pitch", "Round");
			//GotoPreviousScreen ();
			//Debug.Log (state.name + ", " + state.Screen.name);
		}
	}
}
