using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class StageScreen : GameScreen {

	RoundState round;
	protected TimerElement timer;
	protected string playerName;
	protected List<string> players = new List<string> (0);
	float timerDuration = 0;

	static string currentStage = "";
	public static string CurrentStage { 
		get { return currentStage; }
		protected set { currentStage = value;}
	}

	protected bool ThisScreen {
		get { return CurrentStage == name; }
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

	protected virtual string TimerText {
		get { return ""; }
	}

	public StageScreen (GameState state, string name) : base (state, name) {}

	protected void InitStageScreen (float timerDuration) {
		
		Events.instance.AddListener<AllReceiveMessageEvent> (OnAllReceiveMessageEvent);
		Events.instance.AddListener<HostReceiveMessageEvent> (OnHostReceiveMessageEvent);
		Events.instance.AddListener<RoundStartEvent> (OnRoundStartEvent);

		this.timerDuration = timerDuration;
		timer = CreateTimer ("Timer", 1, name);
		
		round = state as RoundState;
		playerName = round.PlayerName;
		
		ScreenElements.AddEnabled ("topLabel", new LabelElement (round.Question, 0, new WhiteTextStyle ()));
		ScreenElements.AddEnabled ("timer", CreateTimer ("Timer", 1, TimerText));
		ScreenElements.AddEnabled ("pool", new BeanPoolElement ());
		ScreenElements.AddEnabled ("pot", new BeanPotElement ());
		ScreenElements.AddDisabled ("roleCard", CreateButton ("View Role Card", 3));
		ScreenElements.AddDisabled ("timesUp", new LabelElement ("", 4, new CenteredWhiteItalicsStyle ()));
		ScreenElements.AddDisabled ("next", CreateBottomButton ("Next", "", "bottomPink", Side.Right));
		timer = ScreenElements.Get<TimerElement> ("timer");
	}

	protected override void OnScreenStartPlayer () {
		CurrentStage = name;
		InitPlayerScreen ();
	}

	protected override void OnScreenStartDecider () {
		CurrentStage = name;
		InitDeciderScreen ();
	}

	protected void InitPlayerScreen () {
		players = round.Players;
		ScreenElements.SuspendUpdating ();
		ScreenElements.Disable ("next");
		ScreenElements.Get<LabelElement> ("topLabel").Content = round.Question;
		ScreenElements.Enable ("roleCard");
		timer.Interactable = false;
		ScreenElements.Disable ("next");
		ScreenElements.EnableUpdating ();
	}

	protected void InitDeciderScreen () {
		players = round.Players;
		ScreenElements.SuspendUpdating ();
		ScreenElements.Disable ("roleCard");
		ScreenElements.Get<LabelElement> ("topLabel").Content = Copy.GetInstructions (name);
		timer.Content = TimerText;
		timer.Interactable = true;
		ScreenElements.Disable ("next");
		ScreenElements.EnableUpdating ();
	}

	protected override void OnButtonPress (ButtonPressEvent e) {
		switch (e.id) {
			case "View Role Card": GotoScreen ("Role"); break;
			case "Next": OnPressNext (); break;
			case "Timer": HandleTimerPress (); break;
		}
	}

	protected virtual void OnPressNext () {
		if (!Timer.instance.CountingDown)
			GameStateController.instance.AllPlayersGotoNextScreen ();
	}

	void OnRoundStartEvent (RoundStartEvent e) {
		ScreenElements.Disable ("timesUp");
	}

	void HandleTimerPress () {
		if (Player.instance.IsDecider) {
			lastPressedId = "";
			if (StartTimer ()) {
				Timer.instance.AllStartCountDown (timerDuration);
			}
		} 
	}

	protected virtual bool StartTimer () { 
		if (timer.Interactable) {
			timer.Interactable = false;
			return true;
		}
		return false; 
	}

	protected virtual void OnAllReceiveMessageEvent (AllReceiveMessageEvent e) {}
	protected virtual void OnHostReceiveMessageEvent (HostReceiveMessageEvent e) {}
}
