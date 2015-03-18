using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PitchScreen : StageScreen {
	
	int currentPlayer = -1;
	LabelElement currentPitcher;
	LabelElement nextPitcher;
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

	public PitchScreen (GameState state, string name = "Pitch") : base (state, name) {
		currentPitcher = new LabelElement ("", 5, new WhiteTextStyle ());
		nextPitcher = new LabelElement ("", 6, new WhiteTextStyle ());
		//ScreenElements.AddEnabled ("currentPitcher", currentPitcher);
		//ScreenElements.AddEnabled ("nextPitcher", nextPitcher);
		InitStageScreen (TimerValues.pitch);
	}

	/*protected override void OnScreenStartPlayer () {
		base.OnScreenStartPlayer ();
		if (players[currentPlayer] == Player.instance.Name) {
			ScreenElements.Get<LabelElement> ("timesUp").Content = "Your turn!";
		}
	}*/

	protected override void OnScreenStartDecider () {
		base.OnScreenStartDecider ();
		UpdatePitcherLabels ();
		ScreenElements.Get<LabelElement> ("topLabel").Content = Copy.PitchInstructions (players[0]);
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
			//ScreenElements.Get<LabelElement> ("topLabel").Content = Copy.PitchInstructions (NextPlayer);
			//ScreenElements.Enable ("timesUp");
			ScreenElements.Get<LabelElement> ("topLabel").Content = Copy.PitchTimeInstructions (CurrentPlayer);
		}
	}

	void UpdatePitcherLabels () {
		currentPitcher.Content = CurrentLabel;
		nextPitcher.Content = NextLabel;
		MessageSender.instance.SendMessageToAll ("UpdatePitcher", CurrentPlayer, NextLabel);
	}

	protected override void OnPressNext () {
		if (!Timer.instance.CountingDown)
			GameStateController.instance.AllPlayersGotoScreen ("Deliberate");
	}

	public override void OnScreenEnd () {
		currentPlayer = -1;
	}

	protected override void OnAllReceiveMessageEvent (AllReceiveMessageEvent e) {
		
		if (!ThisScreen) return;

		if (e.id == "UpdatePitcher") {
			currentPitcher.Content = e.message1;
			nextPitcher.Content = e.message2;
			pitching = (e.message1 == Player.instance.Name);
			/*if (pitching) {
				ScreenElements.Get<LabelElement> ("timesUp").Content = "Your turn!";
			}*/
		}

		if (e.id == "YesAddTime") {
			Timer.instance.AllAddSeconds (TimerValues.extraTime);
		}

		if (e.id == "NoAddTime") {
			if (Player.instance.IsDecider) {
				if (currentPlayer >= players.Count-1) {
					ScreenElements.Enable ("next");
					ScreenElements.Enable ("timesUp");
					ScreenElements.Get<LabelElement> ("timesUp").Content = Copy.PitchTimeDecider2;
				} else {
					ScreenElements.Get<LabelElement> ("topLabel").Content = Copy.PitchInstructions (NextPlayer);
					timer.Interactable = true;
				}
			}
		}
	}
}
