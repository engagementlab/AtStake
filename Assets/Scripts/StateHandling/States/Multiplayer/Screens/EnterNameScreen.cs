using UnityEngine;
using System.Collections;

public class EnterNameScreen : GameScreen {

	TextFieldElement tfe;

	public EnterNameScreen (GameState state, string name = "Enter Name") : base (state, name) {
		
		tfe = new TextFieldElement ();
		
		// For testing only!
		#if UNITY_WEBPLAYER
			tfe.content = "Forrest";
		#endif

		#if UNITY_IPHONE
			tfe.content = "Lieutenant Dan";
		#endif

		#if UNITY_EDITOR
			tfe.content = "Jenny";
		#endif

		SetStaticElements (new ScreenElement[] {
			new LabelElement ("Please type your name:"),
			tfe,
			CreateButton ("Enter"),
			CreateBottomButton ("Back")
		});
	}
	
	protected override void OnButtonPress (ButtonPressEvent e) {
		switch (e.id) {
			case "Enter": 
				if (tfe.content != "") {
					Events.instance.Raise (new EnterNameEvent (tfe.content));
					GotoScreen ("Host or Join");
				}
				break;
			case "Back": GotoScreen ("Start", "Start"); break;
		}
	}
}
