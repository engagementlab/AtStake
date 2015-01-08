using UnityEngine;
using System.Collections;

public class FinalScoreboardScreen : GameScreen {
	
	public FinalScoreboardScreen (GameState state, string name = "Final Scoreboard") : base (state, name) {
		
		SetStaticElements (new ScreenElement[] {
			new LabelElement ("Final Scores")
		});
	}
}
