using UnityEngine;
using System.Collections;

public class GameState {

	public readonly string name = "";
	int screenIndex = 0;
	GameScreen[] screens;
	GameScreen screen;
	public GameScreen Screen {
		get { return screen; }
	}

	public GameState (string name) {
		this.name = name;
		this.screens = SetScreens ();
		screenIndex = 0;
		screen = screens[0];
	}

	public virtual GameScreen[] SetScreens () {
		return new GameScreen[0];
	}
	
	void GotoScreen (int index) {
		screenIndex = index;
		screen = screens[index];
		Events.instance.Raise (new ChangeScreenEvent (screen));
	}

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
}
