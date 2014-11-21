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

	public static GameStateController instance;

	void Awake () {
		if (instance == null)
			instance = this;
		states = new GameStates ();
		GotoState (0);

		// Each GameState normally handles this message, but GameStateController is sending it here
		// so that we don't get a bunch of messages at the start of the game
		Events.instance.Raise (new ChangeScreenEvent (Screen));
	}

	void GotoState (int index) {
		stateIndex = index;
		state = states.GetState (index);
		Events.instance.Raise (new ChangeStateEvent (state));
	}

	public void GotoScreen (string screenName, string stateName = "") {
		if (stateName != "")
			GotoState (stateName);
		state.GotoScreen (screenName);
	}

	public void GotoState (string name) {
		GotoState (states.GetStateIndex (name));
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
		GotoState (stateIndex);
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
		GotoState (stateIndex);
		state.GotoLastScreen ();
	}

	/*void Update () {
		// Debugging
		if (Input.GetKeyDown (KeyCode.Space)) {
			//GotoNextScreen ();
			//GotoState ("Round");
			//GotoScreen ("Pitch", "Round");
			//GotoPreviousScreen ();
			//GotoState ("Multiplayer");
			Debug.Log (state.name + ", " + state.Screen.name);
			GotoNextScreen ();
		}
	}*/
}
