using UnityEngine;
using System.Collections;

public class StageScreen : GameScreen {

	TimerElement timer;
	protected string playerName;

	public StageScreen (GameState state, string name) : base (state, name) {

	}

	protected void InitStageScreen (float defaultTime) {
		
		Events.instance.AddListener<PlayerSendMessageEvent> (OnPlayerSendMessageEvent);
		Events.instance.AddListener<OthersSendMessageEvent> (OnOthersSendMessageEvent);

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
		if (Player.instance.MyBeanPool.SubtractBeans (1)) {
			Timer.instance.DeciderAddSeconds (5f);
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
	protected virtual void OnPlayerSendMessageEvent (PlayerSendMessageEvent e) {}
	protected virtual void OnOthersSendMessageEvent (OthersSendMessageEvent e) {}
}
