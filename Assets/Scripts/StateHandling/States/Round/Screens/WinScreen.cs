using UnityEngine;
using System.Collections;

public class WinScreen : GameScreen {

	LabelElement winner;

	public WinScreen (GameState state, string name = "Win") : base (state, name) {
		winner = new LabelElement ("");
		SetStaticElements (new ScreenElement[] {
			winner
		});
	}

	protected override void OnScreenStartPlayer () {
		if (Player.instance.Won) {
			winner.content = "You won this round!";
			Player.instance.MyBeanPool.OnWin ();
		} else {
			winner.content = string.Format ("{0} won this round", Player.instance.WinningPlayer);
		}
	}

	protected override void OnScreenStartDecider () {
		winner.content = string.Format ("{0} won this round", Player.instance.WinningPlayer);
		SetVariableElements (new ScreenElement[] {
			CreateButton ("Next")
		});
	}

	protected override void OnButtonPress (ButtonPressEvent e) {
		if (e.id == "Next") {
			GameStateController.instance.AllPlayersGotoNextScreen ();
		}
	}
}
