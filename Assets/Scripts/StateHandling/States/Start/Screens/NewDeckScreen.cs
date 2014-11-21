using UnityEngine;
using System.Collections;

public class NewDeckScreen : GameScreen {
	
	public NewDeckScreen (string name = "New Deck") : base (name) {
		SetElements (new ScreenElement[] {
			new LabelElement ("Here's where people will build new desks"),
			new ButtonElement ("Back-Deck", "Back")
		});
	}

	public override void OnButtonPressEvent (ButtonPressEvent e) {
		if (e.id == "Back-Deck") {
			GotoScreen ("Start");
		}
	}
}
