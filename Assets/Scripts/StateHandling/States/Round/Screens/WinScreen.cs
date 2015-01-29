using UnityEngine;
using System.Collections;

public class WinScreen : GameScreen {

	LabelElement winner;

	public WinScreen (GameState state, string name = "Win") : base (state, name) {
		winner = new LabelElement ("", 0, new CenterWhiteTextStyle ());
		SetStaticElements (new ScreenElement[] {
			winner,
			new ImageElement ("applause", 1, Color.white)
		});
	}

	protected override void OnScreenStartPlayer () {
		if (Player.instance.Won) {
			winner.Content = "You won this round!";
			Player.instance.MyBeanPool.OnWin ();
		} else {
			winner.Content = string.Format ("{0} won this round", Player.instance.WinningPlayer);
			BeanPotManager.instance.OnLose ();
		}
	}

	protected override void OnScreenStartDecider () {
		BeanPotManager.instance.OnLose ();
		winner.Content = string.Format ("{0} won this round", Player.instance.WinningPlayer);
		SetVariableElements (new ScreenElement[] {
			CreateBottomButton ("Next", "", "bottomPink", Side.Right)
		});
	}

	protected override void OnButtonPress (ButtonPressEvent e) {
		if (e.id == "Next") {
			GameStateController.instance.AllPlayersGotoNextScreen ();
		}
	}
}
