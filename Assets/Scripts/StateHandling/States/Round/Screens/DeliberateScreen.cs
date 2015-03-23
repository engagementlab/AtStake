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
		
		if (!ThisScreen) return;
		
		if (e.id == "YesAddTime" && !Timer.instance.CountingDown) {
			if (!Player.instance.IsDecider) {
				MessageMatcher.instance.RemoveMessage ("NoAddTime");
			}
			if (Player.instance.IsDecider) {
				timer.Interactable = false;
				Timer.instance.AllAddSeconds (TimerValues.extraTime);
				MessageSender.instance.SendMessageToAll ("AcceptAddTime", e.message1);
			}
		}

		if (e.id == "AcceptAddTime") {
			if (e.message1 == Player.instance.Name) {
				Player.instance.MyBeanPool.OnAddTime ();
			}
		}
	}

	protected override void OnMessagesMatchEvent (MessagesMatchEvent e) {
		if (e.id == "NoAddTime" && Player.instance.IsDecider) {
			GameStateController.instance.AllPlayersGotoScreen ("Decide");
		}
	}

	public override void OnScreenEnd () {}
}
