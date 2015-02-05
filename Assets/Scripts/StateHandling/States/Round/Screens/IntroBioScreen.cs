using UnityEngine;
using System.Collections;

public class IntroBioScreen : IntroductionScreen {

	public IntroBioScreen (GameState state, string name = "Bio") : base (state, name) {
		ScreenElements.AddDisabled ("description", new LabelElement ("Have everyone introduce themselves, then press next.", 0));
	}

	protected override void OnScreenStartDecider () {
		ScreenElements.SuspendUpdating ();
		ScreenElements.DisableAll ();
		ScreenElements.Enable ("description");
		ScreenElements.Enable ("next");
		ScreenElements.EnableUpdating ();
	}

	void CreateBio () {
		Player player = Player.instance;
		if (player.MyRole != null) {
			ScreenElements.DisableAll ();
			Role playerRole = player.MyRole;
			CreateBio (player.Name, playerRole.name, playerRole.bio);
		}
	}

	protected override void OnUpdateRoleEvent (UpdateRoleEvent e) {
		if (!Player.instance.IsDecider) {
			CreateBio ();
		}
	}
}
