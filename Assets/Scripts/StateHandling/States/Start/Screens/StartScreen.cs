using UnityEngine;
using System.Collections;

public class StartScreen : GameScreen {

	public StartScreen (GameState state, string name = "Start") : base (state, name) {
		ScreenElements.AddEnabled ("background", new BackgroundElement ("logo", Color.black));
		ScreenElements.AddEnabled ("play", CreateButton ("Play", 0));
		ScreenElements.AddEnabled ("about", CreateButton ("About", 1, "", "green"));
	}
}
