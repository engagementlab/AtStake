using UnityEngine;
using System.Collections;

public class EnterNameScreen : GameScreen {
	
	TextFieldElement textField;

	public EnterNameScreen (GameState state, string name = "Enter Name") : base (state, name) {
		ScreenElements.AddEnabled ("background", new BackgroundElement ("logo", Color.black));
		ScreenElements.AddEnabled ("copy", new LabelElement (Copy.EnterName, 0));
		ScreenElements.AddEnabled ("textfield", new TextFieldElement (1));
		ScreenElements.AddEnabled ("enter", CreateButton ("Enter", 2));
		ScreenElements.AddEnabled ("back", CreateBottomButton ("Back"));
		textField = ScreenElements.Get<TextFieldElement> ("textfield");
	}

	protected override bool CanGotoScreen (string id) {
		if (id == "Enter") {
			if (textField.content == "") {
				lastPressedId = "";
				return false;
			} else {
				Events.instance.Raise (new EnterNameEvent (textField.content));
			}
		}
		return true;
	}
}
