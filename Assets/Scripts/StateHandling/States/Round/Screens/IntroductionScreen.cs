using UnityEngine;
using System.Collections;

public class IntroductionScreen : RoleScreen {

	protected string description = "";

	public IntroductionScreen (GameState state, string name = "Introduction") : base (state, name) {
		//ScreenElements.AddDisabled ("next", CreateBottomButton ("Next", "", "bottomPink", Side.Right));
		ScreenElements.AddEnabled ("next", CreateNextButton ());
	}

	/*protected override void OnButtonPress (ButtonPressEvent e) {
		if (e.id == "Next") {
			GameStateController.instance.AllPlayersGotoNextScreen ();
		}
	}*/
}
