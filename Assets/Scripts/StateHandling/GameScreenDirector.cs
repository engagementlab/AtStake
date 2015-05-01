using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameScreenDirector : MonoBehaviour {

	enum GotoType {
		Default,
		All
	}

	class GotoButton {
		
		public readonly string gotoScreen;
		public readonly string gotoState = "";
		public readonly bool back = false;
		public readonly GotoType gotoType = GotoType.Default;

		public GotoButton (string gotoScreen, bool back) {
			this.gotoScreen = gotoScreen;
			this.back = back;
		}

		public GotoButton (string gotoScreen, string gotoState, GotoType gotoType) {
			this.gotoScreen = gotoScreen;
			this.gotoState = gotoState;
			this.gotoType = gotoType;
		}

		public GotoButton (string gotoScreen, string gotoState="", bool back=false, GotoType gotoType=GotoType.Default) {
			this.gotoScreen = gotoScreen;
			this.gotoState = gotoState;
			this.back = back;
			this.gotoType = gotoType;
		}
	}

	Dictionary<string, Dictionary<string, GotoButton>> screenOrders = new Dictionary<string, Dictionary<string, GotoButton>> ();
	public static GameScreenDirector instance;

	string State {
		get { return GameStateController.instance.State.name; }
	}

	string Screen {
		get { return GameStateController.instance.Screen.name; }
	}

	void Awake () {

		if (instance == null) {
			instance = this;
		}
		
		Events.instance.AddListener<DisconnectedFromServerEvent> (OnDisconnectedFromServerEvent);
		Events.instance.AddListener<FoundGamesEvent> (OnFoundGamesEvent);
		Events.instance.AddListener<NameTakenEvent> (OnNameTakenEvent);
		Events.instance.AddListener<RegisterEvent> (OnRegisterEvent);
		
		screenOrders.Add ("Start", new Dictionary<string, GotoButton> () {
			{ "Play",  new GotoButton ("Enter Name", "Multiplayer") },
			{ "About", new GotoButton ("About") }
		});

		screenOrders.Add ("About", new Dictionary<string, GotoButton> () {
			{ "Back",  new GotoButton ("Start", true) }
		});

		screenOrders.Add ("Enter Name", new Dictionary<string, GotoButton> () {
			{ "Back",  new GotoButton ("Start", "Start", true) },
			{ "Enter", new GotoButton ("Host or Join") }
		});

		screenOrders.Add ("Host or Join", new Dictionary<string, GotoButton> () {
			{ "Back",  new GotoButton ("Enter Name", true) },
			{ "Host",  new GotoButton ("Lobby") }
		});

		screenOrders.Add ("Games List", new Dictionary<string, GotoButton> () {
			{ "Back",  new GotoButton ("Host or Join", true) }
		});

		screenOrders.Add ("Lobby", new Dictionary<string, GotoButton> () {
			{ "Back",  new GotoButton ("Host or Join", true) },
			{ "Play",  new GotoButton ("Choose Deck", "Decider", GotoType.All) }
		});

		screenOrders.Add ("Name Taken", new Dictionary<string, GotoButton> () {
			{ "Back",  new GotoButton ("Games List", true) }
		});

		/*screenOrders.Add ("Choose Deck", new Dictionary<string, GotoButton> () {
		});

		screenOrders.Add ("Choose Decider", new Dictionary<string, GotoButton> () {
		});

		screenOrders.Add ("Scoreboard", new Dictionary<string, GotoButton> () {
		});

		screenOrders.Add ("Bio", new Dictionary<string, GotoButton> () {
		});

		screenOrders.Add ("Agenda", new Dictionary<string, GotoButton> () {
		});

		screenOrders.Add ("Question", new Dictionary<string, GotoButton> () {
		});

		screenOrders.Add ("Brainstorm", new Dictionary<string, GotoButton> () {
		});

		screenOrders.Add ("Pitch", new Dictionary<string, GotoButton> () {
		});

		screenOrders.Add ("Deliberate", new Dictionary<string, GotoButton> () {
		});

		screenOrders.Add ("Decide", new Dictionary<string, GotoButton> () {
		});

		screenOrders.Add ("Win", new Dictionary<string, GotoButton> () {
		});

		screenOrders.Add ("Agenda Item", new Dictionary<string, GotoButton> () {
		});

		screenOrders.Add ("Agenda Results", new Dictionary<string, GotoButton> () {
		});

		screenOrders.Add ("Role", new Dictionary<string, GotoButton> () {
		});

		screenOrders.Add ("Add Time", new Dictionary<string, GotoButton> () {
		});

		screenOrders.Add ("Agenda Wait", new Dictionary<string, GotoButton> () {
		});

		screenOrders.Add ("Final Scoreboard", new Dictionary<string, GotoButton> () {
		});*/
	}

	public bool ButtonPress (GameScreen screen, string button) {
		
		string thisScreen = screen.name;
		// <temp>
		if (!screenOrders.ContainsKey (thisScreen)) return false;
		// </temp>

		GotoButton gotoButton;
		if (screenOrders[thisScreen].TryGetValue (button, out gotoButton)) {
			if (gotoButton.gotoType == GotoType.Default) {
				GameStateController.instance.GotoScreen (
					gotoButton.gotoScreen,
					gotoButton.gotoState,
					gotoButton.back
				);
			} else if (gotoButton.gotoType == GotoType.All) {
				GameStateController.instance.AllPlayersGotoScreen (
					gotoButton.gotoScreen,
					gotoButton.gotoState
				);
			}
			return true;
		} else {
			return false;
		}
	}

	void GotoScreen (string screenName, string stateName="") {
		GameStateController.instance.GotoScreen (screenName, stateName);
	}

	/**
	 *	Events
	 */

	void OnDisconnectedFromServerEvent (DisconnectedFromServerEvent e) {
		Debug.Log ("disconnect");
		if (State != "Start")
			GotoScreen ("Host or Join", "Multiplayer");
	}

	void OnFoundGamesEvent (FoundGamesEvent e) {
		// TODO: Check the hosts length and if it's 1, go straight to the lobby
		if (Screen == "Host or Join") {
			GotoScreen ("Games List");
		}
	}

	void OnNameTakenEvent (NameTakenEvent e) {
		if (Screen == "Games List") {
			GotoScreen ("Name Taken");
		}
	}

	void OnRegisterEvent (RegisterEvent e) {
		if (Screen == "Games List" || Screen == "Name Taken") {
			GotoScreen ("Lobby");
		}
	}
}
