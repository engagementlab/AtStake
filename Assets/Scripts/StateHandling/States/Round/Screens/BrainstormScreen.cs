using UnityEngine;
using System.Collections;

public class BrainstormScreen : StageScreen {
	
	public BrainstormScreen (GameState state, string name = "Brainstorm") : base (state, name) {
		InitStageScreen (TimerValues.brainstorm);
	}

	public override void OnCountDownEnd () {
		if (Player.instance.IsDecider) {
			ScreenElements.Enable ("next");			
		}
	}

	protected override void OnPressNext () {
		if (!Timer.instance.CountingDown)
			GameStateController.instance.AllPlayersGotoScreen ("Pitch");
	}
}
