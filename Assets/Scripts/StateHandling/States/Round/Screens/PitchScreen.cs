using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PitchScreen : StageScreen {
	
	int currentPlayer = -1;
	LabelElement currentPitcher;
	LabelElement nextPitcher;

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

	public PitchScreen (GameState state, string name = "Pitch") : base (state, name) {
		currentPitcher = new LabelElement ("", 5);
		nextPitcher = new LabelElement ("", 6);
		InitStageScreen (TimerValues.pitch);
	}

	protected override void OnScreenStartDecider () {
		InitDeciderScreen ();
		UpdatePitcherLabels ();
		AppendVariableElements (new ScreenElement[] {
			currentPitcher,
			nextPitcher
		});
	}

	protected override void OnScreenStartPlayer () {
		InitPlayerScreen ();
		AppendVariableElements (new ScreenElement[] {
			currentPitcher,
			nextPitcher
		});
	}

	protected override bool StartTimer () {
		
		// Do not start timer if all players have pitched
		if (currentPlayer >= players.Count-1)
			return false;

		if (currentPlayer < players.Count-1) {
			string cp = CurrentPlayer;
			if (cp != "")
				MessageRelayer.instance.SendMessageToPlayer (cp, "DisableAddTime");
			currentPlayer ++;
			UpdatePitcherLabels ();
		}

		return true;
	}

	public override void OnCountDownEnd () {
		MessageRelayer.instance.SendMessageToPlayers ("DisableAddTime");
		MessageRelayer.instance.SendMessageToPlayer (CurrentPlayer, "EnableAddTime");
	}

	void UpdatePitcherLabels () {
		currentPitcher.Content = CurrentLabel;
		nextPitcher.Content = NextLabel;
		MessageRelayer.instance.SendMessageToPlayers (CurrentPlayer, NextLabel);
	}

	protected override void OnPlayersReceiveMessage (string message1, string message2) {
		
		// This whole function is pretty ugly
		if (message1 == "EnableAddTime" || message1 == "DisableAddTime")
			return;

		string playerName = message1;
		if (playerName == Player.instance.Name) {
			currentPitcher.Content = "Your turn!";
		} else {
			if (message1 == "") {
				currentPitcher.Content = "";
			} else {
				currentPitcher.Content = message1 + "'s turn";
			}
		}
		nextPitcher.Content = message2;
	}

	protected override void OnPlayerReceiveMessageEvent (PlayerReceiveMessageEvent e) {
		ToggleEnableAddTime (e.message);
	}

	protected override void OnPressNext () {
		if (!Timer.instance.CountingDown)
			GameStateController.instance.AllPlayersGotoScreen ("Deliberate");
	}
}
