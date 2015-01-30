using UnityEngine;
using System.Collections;

public class IntroAgendaScreen : IntroductionScreen {

	public IntroAgendaScreen (GameState state, string name = "Agenda") : base (state, name) {
		description = "Have everyone silently review their secret agenda, then press next.";
	}

	void CreateAgenda () {
		Player player = Player.instance;
		Role playerRole = player.MyRole;
		AppendVariableElements (RoleAgendaItems (playerRole.MyAgenda.items));
	}

	protected override void OnUpdateRoleEvent (UpdateRoleEvent e) {
		if (!Player.instance.IsDecider) {
			ClearScreen ();
			CreateAgenda ();
			AppendVariableElements (new ScreenElement[] {
				new BeanPotElement (),
				new BeanPoolElement ()
			});
		}
	}
}
