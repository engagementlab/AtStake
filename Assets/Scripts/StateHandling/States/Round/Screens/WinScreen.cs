using UnityEngine;
using System.Collections;

public class WinScreen : GameScreen {

	LabelElement winner;

	public WinScreen (GameState state, string name = "Win") : base (state, name) {
		winner = new LabelElement ("", 0, new CenterWhiteTextStyle ());
		ScreenElements.AddEnabled ("winner", winner);
		ScreenElements.AddEnabled ("applause", new ImageElement ("applause", 1, Color.white));
		ScreenElements.AddDisabled ("next", CreateBottomButton ("Next", "", "bottomPink", Side.Right));
	}

	protected override void OnScreenStartPlayer () {
		if (Player.instance.Won) {
			winner.Content = "You won this round!";
			Player.instance.MyBeanPool.OnWin ();
		} else {
			winner.Content = string.Format ("{0} won this round", Player.instance.WinningPlayer);
			BeanPotManager.instance.OnLose ();
		}
		ScreenElements.Disable ("next");
	}

	protected override void OnScreenStartDecider () {
		BeanPotManager.instance.OnLose ();
		winner.Content = string.Format ("{0} won this round", Player.instance.WinningPlayer);
		ScreenElements.Enable ("next");
	}

	protected override void OnButtonPress (ButtonPressEvent e) {
		if (e.id == "Next") {
			GameStateController.instance.AllPlayersGotoNextScreen ();
		}
	}
}
