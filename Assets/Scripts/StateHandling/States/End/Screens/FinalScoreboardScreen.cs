﻿using UnityEngine;
using System.Collections;

public class FinalScoreboardScreen : GameScreen {
	
	public FinalScoreboardScreen (GameState state, string name = "Final Scoreboard") : base (state, name) {
		Events.instance.AddListener<UpdatedPlayerScoresEvent> (OnUpdatedPlayerScoresEvent);
		SetStaticElements (new ScreenElement[] {
			new LabelElement ("Final Scores", 0)
		});
	}

	public override void OnScreenStart (bool hosting, bool isDecider) {}

	void OnUpdatedPlayerScoresEvent (UpdatedPlayerScoresEvent e) {
		ScreenElement[] se = new ScreenElement[e.playerNames.Length+1];
		for (int i = 0; i < se.Length-1; i ++) {
			se[i] = new LabelElement (string.Format ("{0}: {1} beans", e.playerNames[i], e.playerScores[i]), i+1);
		}
		se[se.Length-1] = CreateButton ("Main Menu", se.Length+1);
		SetVariableElements (se);
	}

	protected override void OnButtonPress (ButtonPressEvent e) {
		if (e.id == "Main Menu") {
			MultiplayerManager.instance.DisconnectGame ();
			GotoScreen ("Start", "Start");
			Events.instance.Raise (new GameEndEvent ());
			ClearScreen ();
		}
	}
}
