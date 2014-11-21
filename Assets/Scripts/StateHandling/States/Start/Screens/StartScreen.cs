using UnityEngine;
using System.Collections;

public class StartScreen : GameScreen {

	public StartScreen (string name = "Start") : base (name) {
		SetElements (new ScreenElement[] {
			new ButtonElement ("Play", "Play"),
			new ButtonElement ("Instructions", "Instructions"),
			new ButtonElement ("Deck", "New Deck"),
			new ButtonElement ("About", "About")
		});
	}

	public override void OnButtonPressEvent (ButtonPressEvent e) {
		switch (e.id) {
			case "Play": GotoScreen ("Enter Name", "Multiplayer"); break;
			case "Instructions": GotoScreen ("Instructions"); break;
			case "Deck": GotoScreen ("New Deck"); break;
			case "About": GotoScreen ("About"); break;
		}
	}
}
