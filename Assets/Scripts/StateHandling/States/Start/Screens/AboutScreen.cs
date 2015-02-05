using UnityEngine;
using System.Collections;

public class AboutScreen : GameScreen {
	
	public AboutScreen (GameState state, string name = "About") : base (state, name) {
		ScreenElements.AddEnabled ("title", new LabelElement ("About @Stake", 0, new HeaderTextStyle ()));
		ScreenElements.AddEnabled ("copy", new LabelElement (Copy.About, 1));
		ScreenElements.AddEnabled ("back", CreateBottomButton ("Back", "", "bottomOrange", Side.Left));
	}

	protected override void OnButtonPress (ButtonPressEvent e) {
		if (e.id == "Back") {
			GotoScreen ("Start");
		}
	}
}
