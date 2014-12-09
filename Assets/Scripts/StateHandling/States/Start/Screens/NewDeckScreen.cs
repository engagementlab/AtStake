using UnityEngine;
using System.Collections;

public class NewDeckScreen : GameScreen {
	
	public NewDeckScreen (GameState state, string name = "New Deck") : base (state, name) {
		SetStaticElements (new ScreenElement[] {
			new LabelElement ("Here's where people will build new desks"),
			CreateButton ("Back")
		});
	}

	public override void OnButtonPress (ButtonPressEvent e) {
		if (e.id == "Back") {
			GotoScreen ("Start");
		}
	}
}
