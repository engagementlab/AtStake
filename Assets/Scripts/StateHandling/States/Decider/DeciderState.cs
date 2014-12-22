using UnityEngine;
using System.Collections;

public class DeciderState : GameState {
	
	public DeciderState (string name = "Decider") : base (name) {
		
	}
	
	public override GameScreen[] SetScreens () {
		return new GameScreen[] {
			new ChooseDeckScreen (this),
			new NewRoundScreen (this),
			new ChooseDeciderScreen (this)
		};
	}
}
