using UnityEngine;
using System.Collections;

public class IntroBioScreen : IntroductionScreen {

	public IntroBioScreen (GameState state, string name = "Bio") : base (state, name) {
		ScreenElements.AddEnabled ("background", new BackgroundElement ("introduce", Color.black));
		ScreenElements.AddDisabled ("description", new LabelElement (Copy.IntroBio, 0, new DefaultCenterTextStyle ()));
	}

	protected override void OnScreenStartDecider () {
		ScreenElements.SuspendUpdating ();
		ScreenElements.DisableAll ();
		ScreenElements.Enable ("background");
		ScreenElements.Enable ("description");
		ScreenElements.Enable ("next");
		ScreenElements.EnableUpdating ();
	}

	protected override void SetBackEnabled () {}

	void CreateBio () {
		Player player = Player.instance;
		if (player.MyRole != null) {
			ScreenElements.DisableAll ();
			ScreenElements.Enable ("background");
			Role playerRole = player.MyRole;
			CreateBio (player.Name, playerRole.name, playerRole.bio);
			ScreenElements.Enable ("next");
		}
	}

	protected override void OnUpdateRoleEvent (UpdateRoleEvent e) {
		if (!Player.instance.IsDecider) {
			CreateBio ();
		}
	}
}
