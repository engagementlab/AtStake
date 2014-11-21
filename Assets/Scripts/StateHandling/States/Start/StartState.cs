using UnityEngine;
using System.Collections;

public class StartState : GameState {

	public StartState (string name = "Start") : base (name) {

	}

	public override GameScreen[] SetScreens () {
		return new GameScreen[] {
			new StartScreen (),
			new InstructionsScreen (),
			new NewDeckScreen (),
			new AboutScreen ()
		};
	}
}
