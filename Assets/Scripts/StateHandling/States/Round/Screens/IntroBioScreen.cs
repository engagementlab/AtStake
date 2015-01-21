using UnityEngine;
using System.Collections;

public class IntroBioScreen : IntroductionScreen {

	public IntroBioScreen (GameState state, string name = "Bio") : base (state, name) {
		description = "Have everyone introduce themselves, then press next.";
	}

	public override void OnScreenStart (bool hosting, bool isDecider) {
		if (isDecider) {
			OnScreenStartDecider ();
		} else {
			ClearScreen ();
			CreateBio ();
		}
	}

	void CreateBio () {
		Player player = Player.instance;
		if (player.MyRole != null) {
			Role playerRole = player.MyRole;
			AppendVariableElements (RoleDescription (player.Name, playerRole.name, playerRole.bio));
		}
	}

	protected override void OnUpdateRoleEvent (UpdateRoleEvent e) {
		if (!Player.instance.IsDecider) {
			ClearScreen ();
			CreateBio ();
		}
	}
}
