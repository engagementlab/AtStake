using UnityEngine;
using System.Collections;

public class IntroductionScreen : RoleScreen {

	protected string description = "";

	public IntroductionScreen (GameState state, string name = "Introduction") : base (state, name) {
		ScreenElements.AddEnabled ("next", CreateNextButton ());
	}
}
