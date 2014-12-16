using UnityEngine;
using System.Collections;

[RequireComponent (typeof (NetworkView))]
public class GameStateController : MonoBehaviour {

	GameStates states;
	int stateIndex = -1;

	// States represent categories of screens
	GameState state;
	public GameState State {
		get { return state; }
	}

	// Screens exist within states
	public GameScreen Screen {
		get {
			// This is necessary because ScreenDrawer is trying to get the screen
			// before it's been set - which seems impossible but somehow isn't?
			if (State == null)
				return states.GetState (0).Screen;
			return State.Screen; 
		}
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
		if (stateIndex == index)
			return;
		stateIndex = index;
		state = states.GetState (index);
		state.OnStateStart ();
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

	public void GotoPreviouslyVisitedScreen () {
		state.GotoPreviouslyVisitedScreen ();
	}

	public void AllPlayersGotoNextScreen () {
		Events.instance.Raise (new HostScheduleMessageEvent ("OnGotoNextScreen"));
		networkView.RPC ("OnSendPlayersToNextScreen", RPCMode.All);
	}

	public void AllPlayersGotoScreen (string screenName, string stateName = "") {

		// Using the MessageRelayer in this way doesn't stop this RPC from firing if another RPC is still
		// being confirmed, but it will prevent other RPCs from firing until this one is confirmed.
		// wow such a great explanation !!!
		Events.instance.Raise (new HostScheduleMessageEvent ("OnGotoScreen"));
		networkView.RPC ("OnSendPlayersToScreen", RPCMode.All, screenName, stateName);
	}

	[RPC]
	void OnSendPlayersToNextScreen () {
		Events.instance.Raise (new ClientConfirmMessageEvent ("OnGotoNextScreen"));
		GameStateController.instance.GotoNextScreen ();
	}

	[RPC]
	void OnSendPlayersToScreen (string screenName, string stateName) {
		Events.instance.Raise (new ClientConfirmMessageEvent ("OnGotoScreen"));
		GameStateController.instance.GotoScreen (screenName, stateName);
	}
}
