﻿using UnityEngine;
using System.Collections;

public class IntroductionScreen : RoleScreen {

	public IntroductionScreen (GameState state, string name = "Introduction") : base (state, name) {}

	public override void OnScreenStart (bool hosting, bool isDecider) {
		if (isDecider) {
			OnScreenStartDecider ();
		}
	}

	protected override void OnScreenStartDecider () {
		SetVariableElements (new ScreenElement[] {
			new LabelElement ("Have everyone introduce themselves, then press next."),
			CreateButton ("Next")
		});
	}

	protected override void AddBackButton () {
		// Don't add a back button
	}

	protected override void OnButtonPress (ButtonPressEvent e) {
		if (e.id == "Next") {
			GameStateController.instance.AllPlayersGotoScreen ("Question");
		}
	}
}
