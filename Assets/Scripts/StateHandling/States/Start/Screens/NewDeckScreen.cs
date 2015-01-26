using UnityEngine;
using System.Collections;

public class NewDeckScreen : GameScreen {
	
	public NewDeckScreen (GameState state, string name = "New Deck") : base (state, name) {
		SetStaticElements (new ScreenElement[] {
			new LabelElement (Copy.NewDeck, 0),
			CreateBottomButton ("Back", "", "bottomOrange", Side.Left)
		});
	}

	protected override void OnButtonPress (ButtonPressEvent e) {
		if (e.id == "Back") {
			GotoScreen ("Start");
		}
	}
}
