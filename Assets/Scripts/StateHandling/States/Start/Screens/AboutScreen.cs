using UnityEngine;
using System.Collections;

public class AboutScreen : GameScreen {
	
	public AboutScreen (GameState state, string name = "About") : base (state, name) {
		ScreenElements.AddEnabled ("title", new LabelElement ("About @Stake", 0, new AboutHeaderTextStyle ()));
		ScreenElements.AddEnabled ("copy", new LabelElement (Copy.About, 1, new WhiteTextLeftStyle ()));
		ScreenElements.AddEnabled ("back", CreateBottomButton ("Back", "", "bottomOrange", Side.Left));
	}
}
