using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameState {

	public readonly string name = "";
	int screenIndex = 0;
	GameScreen[] screens;

	int previouslyVisitedIndex;

	GameScreen screen;
	public GameScreen Screen {
		get { return screen; }
	}

	public GameScreen PrevScreen {
		get { return screens[previouslyVisitedIndex]; }
	}

	public GameState (string name) {
		this.name = name;
		this.screens = SetScreens ();
		screenIndex = 0;
		screen = screens[0];
		previouslyVisitedIndex = screenIndex;
	}

	/**
	*	Public functions
	*/

	public void GotoScreen (string screenName, bool back) {
		for (int i = 0; i < screens.Length; i ++) {
			if (screens[i].name == screenName) {
				GotoScreen (i, back);
				return;
			}
		}
		Debug.LogError ("No screen named " + screenName + " exists in state " + name);
	}

	public void GotoFirstScreen () {
		GotoScreen (0, false);
	}

	public void GotoLastScreen () {
		GotoScreen (screens.Length - 1, false);
	}

	public bool GotoNextScreen () {
		if (screenIndex + 1 > screens.Length - 1)
			return false;
		GotoScreen (screenIndex + 1, false);
		return true;
	}

	public bool GotoPreviousScreen () {
		if (screenIndex == 0)
			return false;
		GotoScreen (screenIndex - 1, true);
		return true;
	}

	public void GotoPreviouslyVisitedScreen () {
		GotoScreen (previouslyVisitedIndex, true);
	}

	/**
	*	Private functions
	*/

	void GotoScreen (int index, bool back) {
		screen.OnScreenEnd ();
		previouslyVisitedIndex = screenIndex;
		screenIndex = index;
		screen = screens[index];
		screen.OnScreenStart (MultiplayerManager.instance.Hosting, Player.instance.IsDecider);
		Events.instance.Raise (new ChangeScreenEvent (screen, back));
	}

	/**
	*	Virtual functions
	*/

	public virtual GameScreen[] SetScreens () {
		return new GameScreen[0];
	}

	public virtual void OnStateStart () {}
}
