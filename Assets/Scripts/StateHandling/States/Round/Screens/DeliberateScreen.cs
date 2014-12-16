using UnityEngine;
using System.Collections;

public class DeliberateScreen : StageScreen {
	
	public DeliberateScreen (GameState state, string name = "Deliberate") : base (state, name) {
		InitStageScreen (10f);
	}
}
