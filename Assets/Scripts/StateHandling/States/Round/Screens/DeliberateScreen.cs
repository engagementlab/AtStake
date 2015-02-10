using UnityEngine;
using System.Collections;

public class DeliberateScreen : StageScreen {
	
	public DeliberateScreen (GameState state, string name = "Deliberate") : base (state, name) {
		Events.instance.AddListener<MessagesMatchEvent> (OnMessagesMatchEvent);
		InitStageScreen (TimerValues.deliberate);
	}

	public override void OnCountDownEnd () {
		if (ThisScreen) {
			if (Player.instance.IsDecider) {
				MessageMatcher.instance.SetMessage ("NoAddTime", "true");
			} else {
				GotoScreen ("Add Time");
			}
		}
	}

	protected override void OnPressNext () {
		if (!Timer.instance.CountingDown)
			GameStateController.instance.AllPlayersGotoScreen ("Decide");
	}

	protected override void OnAllReceiveMessageEvent (AllReceiveMessageEvent e) {
		if (ThisScreen && e.id == "YesAddTime") {
			Timer.instance.AllAddSeconds (TimerValues.extraTime);
		}
	}

	void OnMessagesMatchEvent (MessagesMatchEvent e) {
		if (ThisScreen && e.id == "NoAddTime") {
			GameStateController.instance.AllPlayersGotoScreen ("Decide");
		}
	}
}
