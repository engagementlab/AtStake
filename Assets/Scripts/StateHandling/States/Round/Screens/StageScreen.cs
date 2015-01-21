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
		timer = CreateTimer ("Timer", 2, name);
		
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
		if (!addTimeEnabled) {
			OnDisableAddTime ();
		}
		SetVariableElements (new ScreenElement[] {
			CreateButton ("Role Card", 4)
		});
	}

	protected void InitDeciderScreen () {
		timerEnabled = true;
		players = round.Players;
		timer.Content = name;
		timer.Interactable = true;
		SetVariableElements (new ScreenElement[] {
			new LabelElement (Copy.GetInstructions (name), 3),
			CreateBottomButton ("Next", "", Side.Right)
		});
	}

	protected override void OnButtonPress (ButtonPressEvent e) {
		switch (e.id) {
			case "Role Card": GotoScreen ("Role"); break;
			case "Next": OnPressNext (); break;
			case "Timer": HandleTimerPress (); break;
		}
	}

	void HandleTimerPress () {
		if (Player.instance.IsDecider) {
			if (StartTimer ()) {
				Timer.instance.AllStartCountDown (timerDuration);
			}
		} else {
			AddTime ();
		}
	}

	protected virtual void OnPressNext () {
		if (!Timer.instance.CountingDown)
			GameStateController.instance.AllPlayersGotoNextScreen ();
	}

	public void ToggleEnableAddTime (string message) {
		if (message == "EnableAddTime") {
			OnEnableAddTime ();
		}
		if (message == "DisableAddTime") {
			OnDisableAddTime ();
		}
	}

	protected virtual void OnEnableAddTime () {
		addTimeEnabled = true;
		timer.Content = "+30 Seconds";
		timer.Interactable = true;
	}

	protected virtual void OnDisableAddTime () {
		addTimeEnabled = false;
		timer.Content = name;
		timer.Interactable = false;
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
