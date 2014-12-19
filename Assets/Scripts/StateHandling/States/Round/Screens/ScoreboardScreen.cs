using UnityEngine;
using System.Collections;

public class ScoreboardScreen : GameScreen {
	
	public ScoreboardScreen (GameState state, string name = "Scoreboard") : base (state, name) {
		Events.instance.AddListener<UpdatedPlayerScoresEvent> (OnUpdatedPlayerScoresEvent);
		SetStaticElements (new ScreenElement[] {
			new LabelElement ("Scoreboard"),
		});
	}

	void OnUpdatedPlayerScoresEvent (UpdatedPlayerScoresEvent e) {
		ScreenElement[] se = new ScreenElement[e.playerNames.Length];
		for (int i = 0; i < se.Length; i ++) {
			se[i] = new LabelElement (string.Format ("{0}: {1} beans", e.playerNames[i], e.playerScores[i]));
		}
		SetVariableElements (se);
	}
}
