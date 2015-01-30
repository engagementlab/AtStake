using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class StageScreen : GameScreen {

	RoundState round;
	protected TimerElement timer;
	protected string playerName;
	protected bool addTimeEnabled = false;
	protected List<string> players = new List<string> (0);
	float timerDuration = 0;
	LabelElement topLabel;

	protected bool ThisScreen {
		get { return GameStateController.instance.Screen.name == name; }
	}
	protected bool ThisScreenRole {
		get {
			if (GameStateController.instance.Screen.name == "Role" && 
				GameStateController.instance.PrevScreen.name == name) {
				return true;
			}
			return false;
		}
	}

	public StageScreen (GameState state, string name) : base (state, name) {}

	protected void InitStageScreen (float timerDuration) {
		
		Events.instance.AddListener<PlayerReceiveMessageEvent> (OnPlayerReceiveMessageEvent);
		Events.instance.AddListener<OthersReceiveMessageEvent> (OnOthersReceiveMessageEvent);
		Events.instance.AddListener<PlayersReceiveMessageEvent> (OnPlayersReceiveMessageEvent);
		Events.instance.AddListener<DeciderReceiveMessageEvent> (OnDeciderReceiveMessageEvent);
		Events.instance.AddListener<RoundEndEvent> (OnRoundEndEvent);
		Events.instance.AddListener<GameEndEvent> (OnGameEndEvent);

		this.timerDuration = timerDuration;
		timer = CreateTimer ("Timer", 1, name);
		
		round = state as RoundState;
		playerName = round.PlayerName;
		topLabel = new LabelElement (round.Question, 0);
		
		SetStaticElements (new ScreenElement[] {
			topLabel,
			timer,
			new BeanPoolElement (),
			new BeanPotElement ()
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
		topLabel.Content = round.Question;
		SetVariableElements (new ScreenElement[] {
			CreateButton ("Role Card", 3)
		});
	}

	protected void InitDeciderScreen () {
		players = round.Players;
		topLabel.Content = Copy.GetInstructions (name);
		timer.Content = name;
		timer.Interactable = true;
		SetVariableElements (new ScreenElement[] {
			CreateBottomButton ("Next", "", "bottomPink", Side.Right)
		});
	}

	protected override void OnButtonPress (ButtonPressEvent e) {
		switch (e.id) {
			case "Role Card": GotoScreen ("Role"); break;
			case "Next": OnPressNext (); break;
			case "Timer": HandleTimerPress (); break;
		}
	}

	protected virtual void OnPressNext () {
		if (!Timer.instance.CountingDown)
			GameStateController.instance.AllPlayersGotoNextScreen ();
	}

	void HandleTimerPress () {
		Debug.Log("decider: " + Player.instance.IsDecider);
		if (Player.instance.IsDecider) {
			if (StartTimer ()) {
				Timer.instance.AllStartCountDown (timerDuration);
			} else {
				Debug.Log("can't start timer");
			}
		} else {
			AddTime ();
		}
	}

	protected virtual bool StartTimer () { 
		if (timer.Interactable) {
			timer.Interactable = false;
			return true;
		}
		return false; 
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
		if (!addTimeEnabled) {
			return;
		}
		if (Player.instance.MyBeanPool.OnAddTime ()) {
			MessageRelayer.instance.SendMessageToDecider ("AddTime");
		}
	}

	// Only the Decider should override this
	public override void OnCountDownEnd () {
		if (ThisScreen && Player.instance.IsDecider) 
			MessageRelayer.instance.SendMessageToPlayers ("EnableAddTime");
	}

	protected virtual void OnPlayersReceiveMessageEvent (PlayersReceiveMessageEvent e) {
		if (ThisScreen || ThisScreenRole) 
			OnPlayersReceiveMessage (e.message1, e.message2);
	}

	protected virtual void OnDeciderReceiveMessageEvent (DeciderReceiveMessageEvent e) {
		if (ThisScreen && e.id == "AddTime") {
			Timer.instance.AllAddSeconds (TimerValues.extraTime);
			MessageRelayer.instance.SendMessageToPlayers ("DisableAddTime");
		}
	}

	protected virtual void OnPlayersReceiveMessage (string message1, string message2) {
		ToggleEnableAddTime (message1);
	}

	protected virtual void OnPlayerReceiveMessageEvent (PlayerReceiveMessageEvent e) {}
	protected virtual void OnOthersReceiveMessageEvent (OthersReceiveMessageEvent e) {}

	void OnRoundEndEvent (RoundEndEvent e) {
		addTimeEnabled = false;
	}

	protected virtual void OnGameEndEvent (GameEndEvent e) {
		// not particularly elegant, but this works
		ToggleEnableAddTime ("DisableAddTime");
		timer.Interactable = true;
	}
}
