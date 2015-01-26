﻿using UnityEngine;
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
		Events.instance.AddListener<RoundEndEvent> (OnRoundEndEvent);
	}

	public override void OnStateStart () {
		Events.instance.Raise (new RoundStartEvent ());
		if (roundNumber == 3) {
			roundNumber = 0;
		}
		question = QuestionManager.instance.GetQuestion (roundNumber);
		Player player = Player.instance;
		playerName = player.Name;
		if (roundNumber == 0) player.OnRoundStart ();
		roundNumber ++;
	}
	
	public override GameScreen[] SetScreens () {
		return new GameScreen[] {
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
			new ScoreboardScreen (this),
			new RoleScreen (this)
		};
	}

	void OnRoundEndEvent (RoundEndEvent e) {
		players.Clear ();
	}
}
