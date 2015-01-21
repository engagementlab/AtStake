using UnityEngine;
using System.Collections;

public class DeliberateScreen : StageScreen {
	
	public DeliberateScreen (GameState state, string name = "Deliberate") : base (state, name) {
		InitStageScreen (TimerValues.deliberate);
	}

	protected override void OnPressNext () {
		if (!Timer.instance.CountingDown)
			GameStateController.instance.AllPlayersGotoScreen ("Decide");
	}
}
