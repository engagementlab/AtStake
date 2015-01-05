using UnityEngine;
using System.Collections;

public class BrainstormScreen : StageScreen {
	
	public BrainstormScreen (GameState state, string name = "Brainstorm") : base (state, name) {
		InitStageScreen (10f);
	}

	protected override void OnPressNext () {
		GameStateController.instance.AllPlayersGotoScreen ("Pitch");
	}
}
