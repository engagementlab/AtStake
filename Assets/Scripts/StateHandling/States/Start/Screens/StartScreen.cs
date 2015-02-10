﻿using UnityEngine;
using System.Collections;

public class StartScreen : GameScreen {

	public StartScreen (GameState state, string name = "Start") : base (state, name) {
		ScreenElements.AddEnabled ("play", CreateButton ("Play", 0));
		ScreenElements.AddEnabled ("about", CreateButton ("About", 1, "", "green"));
	}

	protected override void OnButtonPress (ButtonPressEvent e) {
		switch (e.id) {
			case "Play": GotoScreen ("Enter Name", "Multiplayer"); break;
			case "About": GotoScreen ("About"); break;
		}
	}
}
