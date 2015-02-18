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

	public GameScreen PrevScreen {
		get { return State.PrevScreen; }
	}

	public static GameStateController instance;

	void Awake () {
		
		if (instance == null)
			instance = this;
		
		states = new GameStates ();
		GotoState (0);

		// Each GameState normally handles this message, but GameStateController is sending it here
		// so that we don't get a bunch of messages at the start of the game
		Events.instance.Raise (new ChangeScreenEvent (Screen, true));
		Events.instance.AddListener<HostSendMessageEvent> (OnHostSendMessageEvent);
	}

	void GotoState (int index) {
		if (stateIndex == index) {
			state.OnStateStart ();
			return;
		}
		stateIndex = index;
		state = states.GetState (index);
		state.OnStateStart ();
		Events.instance.Raise (new ChangeStateEvent (state));
	}

	public void GotoScreen (string screenName, string stateName="", bool back=false) {
		if (stateName != "")
			GotoState (stateName);
		state.GotoScreen (screenName, back);
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
		MessageSender.instance.ScheduleMessage ("OnGotoNextScreen");
	}

	public void AllPlayersGotoScreen (string screenName, string stateName="") {
		MessageSender.instance.ScheduleMessage (new NetworkMessage ("OnGotoScreen", screenName, stateName));
	}

	void OnHostSendMessageEvent (HostSendMessageEvent e) {
		if (e.name == "OnGotoNextScreen") {
			if (!SendRPC ("OnSendPlayersToNextScreen", RPCMode.All)) {
				OnSendPlayersToNextScreen ();
			}
		} else if (e.name == "OnGotoScreen") {
			if (!SendRPC ("OnSendPlayersToScreen", RPCMode.All, e.message1, e.message2)) {
				OnSendPlayersToScreen (e.message1, e.message2);
			}
		}
	}

	bool SendRPC (string name, RPCMode mode, params object[] args) {
		if (Network.isClient || Network.isServer) {
			networkView.RPC (name, mode, args);
			return true;
		}
		return false;
	}

	[RPC]
	void OnSendPlayersToNextScreen () {
		GotoNextScreen ();
	}

	[RPC]
	void OnSendPlayersToScreen (string screenName, string stateName) {
		GotoScreen (screenName, stateName);
	}
}
