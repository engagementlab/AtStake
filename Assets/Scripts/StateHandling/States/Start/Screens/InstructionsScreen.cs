using UnityEngine;
using System.Collections;

public class InstructionsScreen : GameScreen {
	
	public InstructionsScreen (string name = "Instructions") : base (name) {
		SetElements (new ScreenElement[] {
			new LabelElement ("Instructions on how to play the game:\n1) purchase a new microwave and\n2) reconnect with your older sister"),
			new ButtonElement ("Back-Instructions", "Back")
		});
	}

	public override void OnButtonPressEvent (ButtonPressEvent e) {
		if (e.id == "Back-Instructions") {
			GotoScreen ("Start");
		}
	}
}
