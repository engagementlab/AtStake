using UnityEngine;
using System.Collections;

public class AboutScreen : GameScreen {
	
	public AboutScreen (GameState state, string name = "About") : base (state, name) {
		SetStaticElements (new ScreenElement[] {
			new LabelElement (Copy.About, 0),
			CreateBottomButton ("Back", "", "bottomOrange", Side.Left)
		});
	}

	protected override void OnButtonPress (ButtonPressEvent e) {
		if (e.id == "Back") {
			GotoScreen ("Start");
		}
	}
}
