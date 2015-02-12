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

	public void GotoScreen (string screenName) {
		for (int i = 0; i < screens.Length; i ++) {
			if (screens[i].name == screenName) {
				GotoScreen (i);
				return;
			}
		}
		Debug.LogError ("No screen named " + screenName + " exists in state " + name);
	}

	public void GotoFirstScreen () {
		GotoScreen (0);
	}

	public void GotoLastScreen () {
		GotoScreen (screens.Length - 1);
	}

	public bool GotoNextScreen () {
		if (screenIndex + 1 > screens.Length - 1)
			return false;
		GotoScreen (screenIndex + 1);
		return true;
	}

	public bool GotoPreviousScreen () {
		if (screenIndex == 0)
			return false;
		GotoScreen (screenIndex - 1);
		return true;
	}

	public void GotoPreviouslyVisitedScreen () {
		GotoScreen (previouslyVisitedIndex);
	}

	/**
	*	Private functions
	*/

	void GotoScreen (int index) {
		screen.OnScreenEnd ();
		previouslyVisitedIndex = screenIndex;
		screenIndex = index;
		screen = screens[index];
		screen.OnScreenStart (MultiplayerManager.instance.Hosting, Player.instance.IsDecider);
		Events.instance.Raise (new ChangeScreenEvent (screen));
	}

	/**
	*	Virtual functions
	*/

	public virtual GameScreen[] SetScreens () {
		return new GameScreen[0];
	}

	public virtual void OnStateStart () {}
}
