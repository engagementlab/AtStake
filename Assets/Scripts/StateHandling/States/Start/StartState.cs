using UnityEngine;
using System.Collections;

public class StartState : GameState {

	public StartState (string name = "Start") : base (name) {

	}

	public override GameScreen[] SetScreens () {
		return new GameScreen[] {
			new StartScreen (this),
			new InstructionsScreen (this),
			new NewDeckScreen (this),
			new AboutScreen (this)
		};
	}
}
