using UnityEngine;
using System.Collections;

public class IntroductionScreen : RoleScreen {

	protected string description = "";

	public IntroductionScreen (GameState state, string name = "Introduction") : base (state, name) {}

	public override void OnScreenStart (bool hosting, bool isDecider) {
		if (isDecider) {
			OnScreenStartDecider ();
		}
	}

	protected override void OnScreenStartDecider () {
		SetVariableElements (new ScreenElement[] {
			new BeanPoolElement (),
			new BeanPotElement (),
			new LabelElement (description, 0),
			CreateBottomButton ("Next", "", "bottomPink", Side.Right)
		});
	}

	protected override void AddBackButton () {
		// Don't add a back button
	}

	protected override void OnButtonPress (ButtonPressEvent e) {
		if (e.id == "Next") {
			GameStateController.instance.AllPlayersGotoNextScreen ();
		}
	}
}
