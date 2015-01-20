using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class StageScreen : GameScreen {

	RoundState round;
	TimerElement timer;
	protected string playerName;
	protected bool addTimeEnabled = false;
	protected List<string> players = new List<string> (0);
	float timerDuration = 0;
	bool timerEnabled = true;

	public StageScreen (GameState state, string name) : base (state, name) {}

	protected void InitStageScreen (float timerDuration) {
		
		Events.instance.AddListener<PlayerReceiveMessageEvent> (OnPlayerReceiveMessageEvent);
		Events.instance.AddListener<OthersReceiveMessageEvent> (OnOthersReceiveMessageEvent);
		Events.instance.AddListener<PlayersReceiveMessageEvent> (OnPlayersReceiveMessageEvent);
		Events.instance.AddListener<DeciderReceiveMessageEvent> (OnDeciderReceiveMessageEvent);

		this.timerDuration = timerDuration;
		timer = CreateTimer ("Start", 2);
		
		round = state as RoundState;
		playerName = round.PlayerName;
		SetStaticElements (new ScreenElement[] {
			new LabelElement (name, 0),
			new LabelElement (round.Question, 1),
			timer,
			new BeanPoolElement (),
			new BeanPotElement ()
			// scores
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
		addTimeEnabled = false;
		SetVariableElements (new ScreenElement[] {
			CreateButton ("+30 Seconds", 3),
			CreateButton ("Role Card", 4)
		});
	}

	protected void InitDeciderScreen () {
		timerEnabled = true;
		players = round.Players;
		SetVariableElements (new ScreenElement[] {
			new LabelElement (Copy.GetInstructions (name), 3),
			CreateBottomButton ("Next", "", Side.Right)
		});
	}

	protected override void OnButtonPress (ButtonPressEvent e) {
		switch (e.id) {
			case "Role Card": GotoScreen ("Role"); break;
			case "+30 Seconds": AddTime (); break;
			case "Start": if (StartTimer ()) {
							Timer.instance.AllStartCountDown (timerDuration);
						} break;
			case "Next": OnPressNext (); break;
		}
	}

	protected virtual void OnPressNext () {
		GameStateController.instance.AllPlayersGotoNextScreen ();
	}

	public void ToggleEnableAddTime (string message) {
		if (message == "EnableAddTime")
			addTimeEnabled = true;
		if (message == "DisableAddTime")
			addTimeEnabled = false;
	}

	// Only Players add time
	void AddTime () {
		if (!addTimeEnabled)
			return;
		if (Player.instance.MyBeanPool.OnAddTime ()) {
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
		if (e.id == "AddTime") {
			Timer.instance.AllAddSeconds (TimerValues.extraTime);
			MessageRelayer.instance.SendMessageToPlayers ("DisableAddTime");
		}
	}

	protected virtual void OnPlayersReceiveMessage (string message1, string message2) {
		ToggleEnableAddTime (message1);
	}

	protected virtual bool StartTimer () { 
		if (timerEnabled) {
			timerEnabled = false;
			return true;
		}
		return false; 
	}
	protected virtual void OnPlayerReceiveMessageEvent (PlayerReceiveMessageEvent e) {}
	protected virtual void OnOthersReceiveMessageEvent (OthersReceiveMessageEvent e) {}
}
