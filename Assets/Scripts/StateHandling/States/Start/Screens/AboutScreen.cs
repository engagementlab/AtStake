using UnityEngine;
using System.Collections;

public class AboutScreen : GameScreen {
	
	public AboutScreen (string name = "About") : base (name) {
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
