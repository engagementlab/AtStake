using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RoundState : GameState {
	
	int roundNumber = 0;
	public int RoundNumber {
		get { return roundNumber; }
	}

	string question;
	public string Question {
		get { return question; }
	}

	string playerName;
	public string PlayerName {
		get { return playerName; }
	}

	List<string> players = new List<string>(0);
	public List<string> Players {
		get {
			if (players.Count == 0) {
				foreach (string player in MultiplayerManager.instance.Players) {
					players.Add (player);
				}
				if (Player.instance.IsDecider)
					players.Remove (Player.instance.Name);
			}
			return players;
		}
	}

	public RoundState (string name = "Round") : base (name) {
		Events.instance.AddListener<GameEndEvent> (OnGameEndEvent);
		Events.instance.AddListener<SelectDeciderEvent> (OnSelectDeciderEvent);
	}

	public override void OnStateStart () {
		
		// Reset the player list and assign a new Decider
		players.Clear ();
		if (Player.instance.Won) {
			// DeciderSelectionManager.instance.SetDecider (Player.instance.Name);
			Player.instance.deciderManager.SetDecider (Player.instance.Name);
		} else if (roundNumber == 0) {
			OnSelectDeciderEvent (null);
		}
	}
	
	public override GameScreen[] SetScreens () {
		return new GameScreen[] {
			new ScoreboardScreen (this),
			new IntroBioScreen (this),
			new IntroAgendaScreen (this),
			new QuestionScreen (this),
			new BrainstormScreen (this),
			new PitchScreen (this),
			new DeliberateScreen (this),
			new DecideScreen (this),
			new WinScreen (this),
			new AgendaScreen (this),
			new AgendaResultsScreen (this),
			new RoleScreen (this),
			new AddTimeScreen (this),
			new AgendaWaitScreen (this)
		};
	}

	void OnGameEndEvent (GameEndEvent e) {
		roundNumber = 0;
		players.Clear ();
	}

	void OnSelectDeciderEvent (SelectDeciderEvent e) {
		// Set the round number & question for the round
		Events.instance.Raise (new RoundStartEvent ());
		if (roundNumber == 3) {
			roundNumber = 0;
		}
		question = QuestionManager.instance.GetQuestion (roundNumber);
		
		// Update the player
		Player player = Player.instance;
		playerName = player.Name;
		if (roundNumber == 0) player.OnRoundStart ();
		roundNumber ++;
	}
}
