﻿using UnityEngine;
using System.Collections;

public class QuestionScreen : GameScreen {
	
	LabelElement title;

	public QuestionScreen (GameState state, string name = "Question") : base (state, name) {
		title = new LabelElement ("Round");
		SetStaticElements (new ScreenElement[] {
			title
		});	
	}

	public override void OnScreenStart (bool hosting, bool isDecider) {
		
		RoundState round = state as RoundState;
		int roundNumber = round.RoundNumber;
		title.Content = string.Format("Round {0}", roundNumber);

		ScreenElement[] se = new ScreenElement[isDecider ? 2 : 1];
		se[0] = new LabelElement (round.Question);
		if (isDecider) {
			se[1] = CreateBottomButton ("Next", "", Side.Right);
		}

		SetVariableElements (se);
	}

	protected override void OnButtonPress (ButtonPressEvent e) {
		if (e.id == "Next") {
			GameStateController.instance.AllPlayersGotoScreen ("Brainstorm");
		}
	}
}
