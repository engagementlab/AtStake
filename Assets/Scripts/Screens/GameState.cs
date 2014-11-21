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

	public GameState (string name, GameScreen[] screens) {
		this.name = name;
		this.screens = screens;
		GotoFirstScreen ();
	}

	public void GotoScreen (string screenName) {
		for (int i = 0; i < screens.Length; i ++) {
			if (screens[i].name == screenName) {
				screenIndex = i;
				screen = screens[i];
				return;
			}
		}
		Debug.LogError ("No screen named " + screenName + " exists in state " + name);
	}

	public void GotoFirstScreen () {
		screenIndex = 0;
		screen = screens[screenIndex];
	}

	public void GotoLastScreen () {
		screenIndex = screens.Length - 1;
		screen = screens[screenIndex];
	}

	public bool GotoNextScreen () {
		if (screenIndex + 1 > screens.Length - 1)
			return false;
		screenIndex ++;
		screen = screens[screenIndex];
		return true;
	}

	public bool GotoPreviousScreen () {
		if (screenIndex == 0)
			return false;
		screenIndex --;
		screen = screens[screenIndex];
		return true;
	}
}
