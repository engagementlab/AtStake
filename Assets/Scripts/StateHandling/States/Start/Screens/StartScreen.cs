using UnityEngine;
using System.Collections;

public class StartScreen : GameScreen {

	public StartScreen (string name = "Start") : base (name) {
		SetStaticElements (new ScreenElement[] {
			CreateButton ("Play"),
			CreateButton ("Instructions"),
			CreateButton ("Deck"),
			CreateButton ("About")
		});
	}

	public override void OnButtonPress (ButtonPressEvent e) {
		switch (e.id) {
			case "Play": GotoScreen ("Enter Name", "Multiplayer"); break;
			case "Instructions": GotoScreen ("Instructions"); break;
			case "Deck": GotoScreen ("New Deck"); break;
			case "About": GotoScreen ("About"); break;
		}
	}
}
