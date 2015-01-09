using UnityEngine;
using System.Collections;

public class BrainstormScreen : StageScreen {
	
	public BrainstormScreen (GameState state, string name = "Brainstorm") : base (state, name) {
		InitStageScreen (TimerValues.brainstorm);
	}

	protected override void OnPressNext () {
		GameStateController.instance.AllPlayersGotoScreen ("Pitch");
	}
}
