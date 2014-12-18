﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class StageScreen : GameScreen {

	RoundState round;
	TimerElement timer;
	protected string playerName;
	protected bool addTimeEnabled = false;
	protected List<string> players = new List<string> (0);

	public StageScreen (GameState state, string name) : base (state, name) {

	}

	protected void InitStageScreen (float defaultTime) {
		
		Events.instance.AddListener<PlayerReceiveMessageEvent> (OnPlayerReceiveMessageEvent);
		Events.instance.AddListener<OthersReceiveMessageEvent> (OnOthersReceiveMessageEvent);
		Events.instance.AddListener<PlayersReceiveMessageEvent> (OnPlayersReceiveMessageEvent);
		Events.instance.AddListener<DeciderReceiveMessageEvent> (OnDeciderReceiveMessageEvent);

		timer = CreateTimer (defaultTime);
		
		round = state as RoundState;
		playerName = round.PlayerName;
		SetStaticElements (new ScreenElement[] {
			new LabelElement (name),
			new LabelElement (round.Question),
			timer
			// pot
			// score
			// stage progress
		});
	}

	protected override void OnScreenStartPlayer () {
		InitPlayerScreen ();
	}

	protected override void OnScreenStartDecider () {
		players = round.Players;
		InitDeciderScreen ();
	}

	protected void InitPlayerScreen () {
		SetVariableElements (new ScreenElement[] {
			CreateButton ("+30 Seconds"),
			CreateButton ("Role Card")
		});
	}

	protected void InitDeciderScreen () {
		SetVariableElements (new ScreenElement[] {
			new LabelElement (Copy.GetInstructions (name)),
			CreateButton ("Start Timer"),
			CreateButton ("Next")
		});
	}

	protected override void OnButtonPress (ButtonPressEvent e) {
		switch (e.id) {
			case "Role Card": GotoScreen ("Role"); break;
			case "+30 Seconds": AddTime (); break;
			case "Start Timer": if (StartTimer ()) {
									timer.StartCountDown (); 
								}
								break;
			case "Next": GameStateController.instance.AllPlayersGotoNextScreen (); break;
		}
	}

	public void ToggleAddTime (string message) {
		if (message == "EnableAddTime")
			addTimeEnabled = true;
		if (message == "DisableAddTime")
			addTimeEnabled = false;
	}

	// Only Players add time
	void AddTime () {
		if (!addTimeEnabled)
			return;
		if (Player.instance.MyBeanPool.SubtractBeans (1)) {
			MessageRelayer.instance.SendMessageToDecider ("AddTime");
		}
	}

	// Only the Decider should override this
	public override void OnCountDownEnd () {
		MessageRelayer.instance.SendMessageToPlayers ("EnableAddTime");
	}

	protected virtual void OnPlayersReceiveMessageEvent (PlayersReceiveMessageEvent e) {
		OnPlayersReceiveMessage (e.message1, e.message2);
	}

	protected virtual void OnDeciderReceiveMessageEvent (DeciderReceiveMessageEvent e) {
		if (e.message1 == "AddTime") {
			Timer.instance.AddSeconds (5f);
			MessageRelayer.instance.SendMessageToPlayers ("DisableAddTime");
		}
	}

	protected virtual void OnPlayersReceiveMessage (string message1, string message2) {
		ToggleAddTime (message1);
	}

	protected virtual bool StartTimer () { return true; }
	protected virtual void OnPlayerReceiveMessageEvent (PlayerReceiveMessageEvent e) {}
	protected virtual void OnOthersReceiveMessageEvent (OthersReceiveMessageEvent e) {}
}
