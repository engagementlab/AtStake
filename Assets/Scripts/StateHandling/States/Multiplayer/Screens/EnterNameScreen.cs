using UnityEngine;
using System.Collections;

public class EnterNameScreen : GameScreen {

	TextFieldElement tfe;

	public EnterNameScreen (string name = "Enter Name") : base (name) {
		tfe = new TextFieldElement ();
		SetElements (new ScreenElement[] {
			new LabelElement ("Please type your name:"),
			tfe,
			new ButtonElement ("Enter-Enter-Name", "Enter"),
			new ButtonElement ("Back-Enter-Name", "Back")
		});
	}
	
	public override void OnButtonPressEvent (ButtonPressEvent e) {
		switch (e.id) {
			case "Enter-Enter-Name": 
				if (tfe.content != "") {
					MultiplayerManager.instance.PlayerName = tfe.content;
					GotoScreen ("Host or Join");
				}
				break;
			case "Back-Enter-Name": GotoScreen ("Start", "Start"); break;
		}
	}
}
