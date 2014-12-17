﻿using UnityEngine;
using System.Collections;

public class StageScreen : GameScreen {

	TimerElement timer;
	protected string playerName;
	protected bool addTimeEnabled = false;

	public StageScreen (GameState state, string name) : base (state, name) {

	}

	protected void InitStageScreen (float defaultTime) {
		
		Events.instance.AddListener<PlayerReceiveMessageEvent> (OnPlayerReceiveMessageEvent);
		Events.instance.AddListener<OthersReceiveMessageEvent> (OnOthersReceiveMessageEvent);
		Events.instance.AddListener<PlayersReceiveMessageEvent> (OnPlayersReceiveMessageEvent);
		Events.instance.AddListener<DeciderReceiveMessageEvent> (OnDeciderReceiveMessageEvent);

		timer = CreateTimer (defaultTime);
		
		RoundState round = state as RoundState;
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

	void AddTime () {
		if (!addTimeEnabled)
			return;
		if (Player.instance.MyBeanPool.SubtractBeans (1)) {
			Timer.instance.DeciderAddSeconds (5f);
			addTimeEnabled = false;
		}
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

	protected virtual bool StartTimer () { return true; }
	protected virtual void OnPlayerReceiveMessageEvent (PlayerReceiveMessageEvent e) {}
	protected virtual void OnOthersReceiveMessageEvent (OthersReceiveMessageEvent e) {}
	protected virtual void OnPlayersReceiveMessageEvent (PlayersReceiveMessageEvent e) {}
	protected virtual void OnDeciderReceiveMessageEvent (DeciderReceiveMessageEvent e) {}
}
