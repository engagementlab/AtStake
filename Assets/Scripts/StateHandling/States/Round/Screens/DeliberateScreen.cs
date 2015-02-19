using UnityEngine;
using System.Collections;

public class DeliberateScreen : StageScreen {
	
	protected override string TimerText {
		get { return "Deliberate!"; }
	}

	public DeliberateScreen (GameState state, string name = "Deliberate") : base (state, name) {
		Events.instance.AddListener<MessagesMatchEvent> (OnMessagesMatchEvent);
		InitStageScreen (TimerValues.deliberate);
	}

	public override void OnCountDownEnd () {
		if (ThisScreen) {
			if (Player.instance.IsDecider) {
				ScreenElements.Enable ("timesUp");
				MessageMatcher.instance.SetMessage ("NoAddTime", "true");
			} else {
				GotoScreen ("Add Time");
			}
		}
	}

	protected override void OnAllReceiveMessageEvent (AllReceiveMessageEvent e) {
		if (ThisScreen && e.id == "YesAddTime") {
			Timer.instance.AllAddSeconds (TimerValues.extraTime);
		}
	}

	protected override void OnMessagesMatchEvent (MessagesMatchEvent e) {
		if (e.id == "NoAddTime" && Player.instance.IsDecider) {
			GameStateController.instance.AllPlayersGotoScreen ("Decide");
		}
	}

	public override void OnScreenEnd () {}
}
