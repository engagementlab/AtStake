using UnityEngine;
using System.Collections;

public class RoundState : GameState {
	
	public RoundState (string name = "Round") : base (name) {
		
	}
	
	public override GameScreen[] SetScreens () {
		return new GameScreen[] {
			new BrainstormScreen (),
			new PitchScreen (),
			new DeliberateScreen (),
			new DecideScreen (),
			new AgendaScreen (),
			new ScoreboardScreen ()
		};
	}
}
