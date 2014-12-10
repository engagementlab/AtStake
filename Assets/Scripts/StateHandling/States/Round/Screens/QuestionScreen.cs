using UnityEngine;
using System.Collections;

public class QuestionScreen : GameScreen {
	
	LabelElement title;

	public QuestionScreen (GameState state, string name = "Question") : base (state, name) {
		title = new LabelElement ("Round");
		SetStaticElements (new ScreenElement[] {
			title
		});	
	}

	public override void OnScreenStart () {
		
		RoundState round = state as RoundState;
		int roundNumber = round.RoundNumber;
		title.content = string.Format("Round {0}", roundNumber);

		SetVariableElements (new ScreenElement[] {
			new LabelElement (round.Question)
		});
	}
}
