using UnityEngine;
using System.Collections;

public class InstructionsScreen : GameScreen {
	
	public InstructionsScreen (GameState state, string name = "Instructions") : base (state, name) {
		SetStaticElements (new ScreenElement[] {
			new LabelElement (Copy.Instructions, 0),
			CreateBottomButton ("Back", "", "bottomOrange", Side.Left)
		});
	}

	protected override void OnButtonPress (ButtonPressEvent e) {
		if (e.id == "Back") {
			GotoScreen ("Start");
		}
	}
}
