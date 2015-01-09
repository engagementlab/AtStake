using UnityEngine;
using System.Collections;

public class IntroBioScreen : IntroductionScreen {

	public IntroBioScreen (GameState state, string name = "Bio") : base (state, name) {
		description = "Have everyone introduce themselves, then press next.";
	}

	void CreateBio () {
		Player player = Player.instance;
		Role playerRole = player.MyRole;
		AppendVariableElements (RoleDescription (player.Name, playerRole.name, playerRole.bio));
	}

	protected override void OnUpdateRoleEvent (UpdateRoleEvent e) {
		if (!Player.instance.IsDecider) {
			ClearScreen ();
			CreateBio ();
		}
	}
}
