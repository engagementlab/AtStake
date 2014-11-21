using UnityEngine;
using System.Collections;

public class EndState : GameState {
	
	public EndState (string name = "End") : base (name) {
		
	}
	
	public override GameScreen[] SetScreens () {
		return new GameScreen[] {
			new FinalScoreboardScreen ()

		};
	}
}
