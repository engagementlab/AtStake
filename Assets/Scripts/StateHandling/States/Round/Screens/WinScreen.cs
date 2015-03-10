using UnityEngine;
using System.Collections;

public class WinScreen : GameScreen {

	LabelElement winner;

	public WinScreen (GameState state, string name = "Win") : base (state, name) {
		winner = new LabelElement ("", 0, new WhiteTextStyle ());
		ScreenElements.AddEnabled ("winner", winner);
		ScreenElements.AddEnabled ("applause", new ImageElement ("applause", 1, Color.white));
		ScreenElements.AddEnabled ("next", CreateNextButton ());
	}

	protected override void OnScreenStartPlayer () {
		if (AgendaVotingStyle.All) ScreenElements.Disable ("next");
		if (Player.instance.Won) {
			winner.Content = Copy.YouWin; 
			Player.instance.MyBeanPool.OnWin ();
		} else {
			winner.Content = Copy.PlayerWin;
			BeanPotManager.instance.OnLose ();
		}
	}

	protected override void OnScreenStartDecider () {
		if (AgendaVotingStyle.All) ScreenElements.Enable ("next");
		BeanPotManager.instance.OnLose ();
		winner.Content = Copy.PlayerWin;
	}

	protected override void OnButtonPress (ButtonPressEvent e) {
		if (AgendaVotingStyle.All) {
			if (e.id == "Next") {
				if (AgendaVotingStyle.All) {
					GameStateController.instance.AllPlayersGotoNextScreen ();
				} 
			}
		}
	}

	protected override void OnMessagesMatch () {
		if (AgendaVotingStyle.Decider) {
			if (Player.instance.IsDecider) {
				GameStateController.instance.GotoNextScreen ();
			} else {
				GotoScreen ("Agenda Wait");
			}
		}
	}
}
