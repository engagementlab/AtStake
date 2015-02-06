using UnityEngine;
using System.Collections;

public class EnterNameScreen : GameScreen {
	
	public EnterNameScreen (GameState state, string name = "Enter Name") : base (state, name) {
		ScreenElements.AddEnabled ("copy", new LabelElement ("Please type your name:", 0));
		ScreenElements.AddEnabled ("textfield", new TextFieldElement (1));
		ScreenElements.AddEnabled ("enter", CreateButton ("Enter", 2));
		ScreenElements.AddEnabled ("back", CreateBottomButton ("Back"));
	}
	
	protected override void OnButtonPress (ButtonPressEvent e) {
		switch (e.id) {
			case "Enter": 
				TextFieldElement tfe = ScreenElements.Get<TextFieldElement> ("textfield");
				if (tfe.content != "") {
					Events.instance.Raise (new EnterNameEvent (tfe.content));
					GotoScreen ("Host or Join");
				}
				break;
			case "Back": GotoScreen ("Start", "Start"); break;
		}
	}
}
