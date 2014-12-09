using UnityEngine;
using System.Collections;

public class EnterNameScreen : GameScreen {

	TextFieldElement tfe;

	public EnterNameScreen (string name = "Enter Name") : base (name) {
		tfe = new TextFieldElement ();
		SetStaticElements (new ScreenElement[] {
			new LabelElement ("Please type your name:"),
			tfe,
			CreateButton ("Enter"),
			CreateButton ("Back")
		});
	}
	
	public override void OnButtonPress (ButtonPressEvent e) {
		switch (e.id) {
			case "Enter": 
				if (tfe.content != "") {
					MultiplayerManager.instance.PlayerName = tfe.content;
					GotoScreen ("Host or Join");
				}
				break;
			case "Back": GotoScreen ("Start", "Start"); break;
		}
	}
}
