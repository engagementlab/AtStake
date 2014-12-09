using UnityEngine;
using System.Collections;

public class AboutScreen : GameScreen {
	
	public AboutScreen (GameState state, string name = "About") : base (state, name) {
		SetStaticElements (new ScreenElement[] {
			new LabelElement ("@Stake is a game designed by the Engagement Lab at Emerson College"),
			CreateButton ("Back")
		});
	}

	public override void OnButtonPress (ButtonPressEvent e) {
		if (e.id == "Back") {
			GotoScreen ("Start");
		}
	}
}
