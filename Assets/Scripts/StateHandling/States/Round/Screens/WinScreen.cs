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
		if (AgendaVotingType.All) ScreenElements.Disable ("next");
		if (Player.instance.Won) {
			winner.Content = "You won this round!";
			Player.instance.MyBeanPool.OnWin ();
		} else {
			winner.Content = string.Format ("{0} won this round", Player.instance.WinningPlayer);
			BeanPotManager.instance.OnLose ();
		}
	}

	protected override void OnScreenStartDecider () {
		if (AgendaVotingType.All) ScreenElements.Enable ("next");
		BeanPotManager.instance.OnLose ();
		winner.Content = string.Format ("{0} won this round", Player.instance.WinningPlayer);
	}

	protected override void OnButtonPress (ButtonPressEvent e) {
		if (AgendaVotingType.All) {
			if (e.id == "Next") {
				if (AgendaVotingType.All) {
					GameStateController.instance.AllPlayersGotoNextScreen ();
				} 
			}
		}
	}

	protected override void OnMessagesMatch () {
		if (AgendaVotingType.Decider) {
			if (Player.instance.IsDecider) {
				GameStateController.instance.GotoNextScreen ();
			} else {
				GotoScreen ("Agenda Wait");
			}
		}
	}
}
