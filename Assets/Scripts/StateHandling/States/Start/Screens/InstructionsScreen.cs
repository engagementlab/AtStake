using UnityEngine;
using System.Collections;

public class InstructionsScreen : GameScreen {
	
	public InstructionsScreen (GameState state, string name = "Instructions") : base (state, name) {
		SetStaticElements (new ScreenElement[] {
			new LabelElement ("Instructions on how to play the game:\n1) purchase a new microwave and\n2) reconnect with your older sister"),
			CreateButton ("Back")
		});
	}

	public override void OnButtonPress (ButtonPressEvent e) {
		if (e.id == "Back") {
			GotoScreen ("Start");
		}
	}
}
