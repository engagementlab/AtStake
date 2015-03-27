using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PitchScreen : StageScreen {
	
	int currentPlayer = -1;
	bool pitching = false;
	
	string CurrentPlayer {
		get {
			if (players.Count == 0 || currentPlayer == -1)
				return "";
			else
				return players[currentPlayer];
		}
	}

	string NextPlayer {
		get {
			if (players.Count == 0 || currentPlayer >= players.Count-1)
				return "";
			else
				return players[currentPlayer+1];
		}
	}

	string CurrentLabel {
		get {
			if (players.Count == 0 || CurrentPlayer == "") {
				return "";
			} else {
				return CurrentPlayer + "'s turn";
			}
		}
	}

	string NextLabel {
		get {
			if (players.Count == 0 || NextPlayer == "")
				return "";
			else
				return "Up next: " + NextPlayer;
		}
	}

	protected override string TimerText {
		get { return "Pitch!"; }
	}

	LabelElement pitcherLabel;

	public PitchScreen (GameState state, string name = "Pitch") : base (state, name) {
		pitcherLabel = new LabelElement ("", 5, new WhiteTextStyle ());
		ScreenElements.AddEnabled ("pitcher", pitcherLabel);
		InitStageScreen (TimerValues.pitch);
		Events.instance.AddListener<RoundStartEvent> (OnRoundStartEvent);
	}

	protected override void OnScreenStartPlayer () {
		base.OnScreenStartPlayer ();
		if (currentPlayer > -1) return;
		if (players[0] == Player.instance.Name) {
			pitcherLabel.Content = "Your turn!";
		} else {
			pitcherLabel.Content = string.Format ("{0}'s turn", players[0]);
		}
		ScreenElements.Enable ("pitcher");
	}

	protected override void OnScreenStartDecider () {
		base.OnScreenStartDecider ();
		UpdatePitcherLabels ();
		ScreenElements.Get<LabelElement> ("topLabel").Content = Copy.PitchInstructions (players[0]);
		ScreenElements.Disable ("pitcher");
	}

	protected override bool StartTimer () {
		
		if (!timer.Interactable) {
			return false;
		}

		currentPlayer ++;
		UpdatePitcherLabels ();
		timer.Interactable = false;
		ScreenElements.Disable ("timesUp");
		return true;
	}

	public override void OnCountDownEnd () {
		if (!ThisScreen) return;
		if (pitching) {
			GotoScreen ("Add Time");
		}
		if (Player.instance.IsDecider) {
			ScreenElements.Get<LabelElement> ("topLabel").Content = Copy.PitchTimeInstructions (CurrentPlayer);
		} 
	}

	void UpdatePitcherLabels () {
		MessageSender.instance.SendMessageToAll ("UpdatePitcher", CurrentPlayer, NextLabel, currentPlayer);
	}

	protected override void OnPressNext () {
		if (!Timer.instance.CountingDown)
			GameStateController.instance.AllPlayersGotoScreen ("Deliberate");
	}

	void OnRoundStartEvent (RoundStartEvent e) {
		currentPlayer = -1;
	}

	protected override void OnAllReceiveMessageEvent (AllReceiveMessageEvent e) {
		
		if (!ThisScreen) return;

		if (e.id == "UpdatePitcher") {
			pitching = (e.message1 == Player.instance.Name);
			if (!Player.instance.IsDecider) {
				currentPlayer = e.val;
			}
		}

		if (e.id == "YesAddTime" && !Timer.instance.CountingDown) {
			if (Player.instance.IsDecider) {
				timer.Interactable = false;
				Timer.instance.AllAddSeconds (TimerValues.extraTime);
				MessageSender.instance.SendMessageToAll ("AcceptAddTime", e.message1);
			}
		}

		if (e.id == "NoAddTime") {
			if (Player.instance.IsDecider) {
				if (currentPlayer >= players.Count-1) {
					ScreenElements.Enable ("next");
					ScreenElements.Enable ("timesUp");
					ScreenElements.Get<LabelElement> ("timesUp").Content = Copy.PitchTimeDecider2;
					MessageSender.instance.SendMessageToAll ("FinishedPitchingPlayers");
				} else {
					ScreenElements.Get<LabelElement> ("topLabel").Content = Copy.PitchInstructions (NextPlayer);
					timer.Interactable = true;
					MessageSender.instance.SendMessageToAll ("NextPitchingPlayer", NextPlayer);
				}
			} 
		}

		if (e.id == "AcceptAddTime") {
			if (e.message1 == Player.instance.Name) {
				Player.instance.MyBeanPool.OnAddTime ();
			}
		}

		if (e.id == "NextPitchingPlayer") {
			if (Player.instance.IsDecider) return;
			string nextPlayer = e.message1;
			if (nextPlayer == Player.instance.Name) {
				pitcherLabel.Content = "Your turn!";
			} else {
				pitcherLabel.Content = string.Format ("{0}'s turn", nextPlayer);
			}
		}

		if (e.id == "FinishedPitchingPlayers") {
			if (Player.instance.IsDecider) return;
			pitcherLabel.Content = "";
		}
	}
}
