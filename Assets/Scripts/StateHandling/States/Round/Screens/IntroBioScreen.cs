using UnityEngine;
using System.Collections;

public class IntroBioScreen : IntroductionScreen {

	public IntroBioScreen (GameState state, string name = "Bio") : base (state, name) {
		//description = "Have everyone introduce themselves, then press next.";
		ScreenElements.AddDisabled ("description", new LabelElement ("Have everyone introduce themselves, then press next.", 0));
	}

	/*public override void OnScreenStart (bool hosting, bool isDecider) {
		if (isDecider) {
			OnScreenStartDecider ();
		} else {
			//ClearScreen ();
			ScreenElements.Clear ();
			CreateBio ();
		}
	}*/

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
			//AppendVariableElements (RoleDescription (player.Name, playerRole.name, playerRole.bio));

		}
	}

	protected override void OnUpdateRoleEvent (UpdateRoleEvent e) {
		if (!Player.instance.IsDecider) {
			//ClearScreen ();
			//ScreenElements.Disable ("description");
			//ScreenElements.Disable ("next");
			CreateBio ();
		}
	}
}
