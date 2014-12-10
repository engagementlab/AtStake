using UnityEngine;
using System.Collections;

public class RoundState : GameState {
	
	int roundNumber = 0;
	public int RoundNumber {
		get { return roundNumber; }
	}

	string question;
	public string Question {
		get { return question; }
	}

	public RoundState (string name = "Round") : base (name) {
		
	}

	public override void OnStateStart () {
		roundNumber ++;
		question = QuestionManager.instance.GetQuestion (roundNumber);
	}
	
	public override GameScreen[] SetScreens () {
		return new GameScreen[] {
			new QuestionScreen (this),
			new BrainstormScreen (this),
			new PitchScreen (this),
			new DeliberateScreen (this),
			new DecideScreen (this),
			new AgendaScreen (this),
			new ScoreboardScreen (this),
			new RoleScreen (this)
		};
	}
}
