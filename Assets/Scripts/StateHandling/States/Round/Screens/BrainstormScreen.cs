using UnityEngine;
using System.Collections;

public class BrainstormScreen : StageScreen {

	protected override string TimerText {
		get { return "Think!"; }
	}
	
	public BrainstormScreen (GameState state, string name = "Brainstorm") : base (state, name) {
		InitStageScreen (TimerValues.brainstorm);
		timer.Content = "Think!";
	}

	public override void OnCountDownEnd () {
		ScreenElements.Enable ("timesUp");
		LabelElement timesUp = ScreenElements.Get<LabelElement> ("timesUp");
		if (Player.instance.IsDecider) {
			timesUp.Content = Copy.BrainstormTimeDecider;
			ScreenElements.Enable ("next");	
		} else {
			timesUp.Content = Copy.BrainstormTimePlayer;
		}
	}

	protected override void OnPressNext () {
		if (!Timer.instance.CountingDown)
			GameStateController.instance.AllPlayersGotoScreen ("Pitch");
	}
}
