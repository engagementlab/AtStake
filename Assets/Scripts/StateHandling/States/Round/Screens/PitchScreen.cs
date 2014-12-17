﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PitchScreen : StageScreen {
	
	List<string> players = new List<string>(0);
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
		currentPitcher = new LabelElement ("");
		nextPitcher = new LabelElement ("");
		InitStageScreen (10f);
	}

	protected override void OnScreenStartDecider () {
		InitDeciderScreen ();

		players = MultiplayerManager.instance.Players;
		players.Remove (Player.instance.Name);
		
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
		MessageRelayer.instance.SendMessageToPlayer (CurrentPlayer, "EnableAddTime");
	}

	void UpdatePitcherLabels () {
		currentPitcher.content = CurrentLabel;
		nextPitcher.content = NextLabel;
		MessageRelayer.instance.SendMessageToPlayers (CurrentPlayer, NextLabel);
	}

	protected override void OnPlayersReceiveMessageEvent (PlayersReceiveMessageEvent e) {
		string playerName = e.message1;
		if (playerName == Player.instance.Name)
			currentPitcher.content = "Your turn!";
		else
			currentPitcher.content = e.message1 + "'s turn";
		nextPitcher.content = e.message2;
	}

	protected override void OnPlayerReceiveMessageEvent (PlayerReceiveMessageEvent e) {
		if (e.message == "EnableAddTime")
			addTimeEnabled = true;
		if (e.message == "DisableAddTime")
			addTimeEnabled = false;
	}
}
