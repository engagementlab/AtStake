using UnityEngine;
using System.Collections;

public class EnterNameScreen : GameScreen {
	
	TextFieldElement textField;
	ButtonElement enterButton;

	public EnterNameScreen (GameState state, string name = "Enter Name") : base (state, name) {
		enterButton = CreateButton ("Enter", 2);
		ScreenElements.AddEnabled ("background", new BackgroundElement ("logo", Color.black));
		ScreenElements.AddEnabled ("copy", new LabelElement (Copy.EnterName, 0));
		ScreenElements.AddEnabled ("textfield", new TextFieldElement (1));
		ScreenElements.AddEnabled ("enter", enterButton);
		ScreenElements.AddEnabled ("back", CreateBottomButton ("Back"));
		textField = ScreenElements.Get<TextFieldElement> ("textfield");
		textField.onUpdateContent += OnUpdateContent;
	}

	public override void OnScreenStart (bool hosting, bool isDecider) {
		SetEnterInteractable ();
	}

	void OnUpdateContent (string content) {
		SetEnterInteractable ();
	}

	protected override bool CanGotoScreen (string id) {
		if (id == "Enter") {
			if (textField.Content == "") {
				lastPressedId = "";
				return false;
			} else {
				Events.instance.Raise (new EnterNameEvent (textField.Content));
			}
		}
		return true;
	}

	void SetEnterInteractable () {
		if (textField.Content == "") {
			enterButton.Interactable = false;
		} else {
			enterButton.Interactable = true;
		}		
	}
}
