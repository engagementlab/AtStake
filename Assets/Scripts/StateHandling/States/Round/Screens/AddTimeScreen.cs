using UnityEngine;
using System.Collections;

public class AddTimeScreen : GameScreen {

	public AddTimeScreen (GameState state, string name = "Add Time") : base (state, name) {
		ScreenElements.AddEnabled ("instructions", "Time's up but for a couple beans you can get 30 more seconds :)");
		ScreenElements.AddEnabled ("add", CreateButton ("+30 Seconds", 1));
		ScreenElements.AddEnabled ("done", CreateButton ("I'm Done", 2));
	}

	protected override void OnButtonPress (ButtonPressEvent e) {
		switch (e.id) {
			case "+30 Seconds": break;
			case "I'm Done": break;
		}
	}
}