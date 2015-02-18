using UnityEngine;
using System.Collections;

public class IntroAgendaScreen : IntroductionScreen {

	public IntroAgendaScreen (GameState state, string name = "Agenda") : base (state, name) {
		ScreenElements.AddDisabled ("description", new LabelElement (Copy.IntroAgenda, 0));
	}

	protected override void OnScreenStartDecider () {
		ScreenElements.SuspendUpdating ();
		ScreenElements.DisableAll ();
		ScreenElements.Enable ("description");
		ScreenElements.Enable ("next");
		ScreenElements.EnableUpdating ();
	}

	void CreateAgenda () {
		ScreenElements.DisableAll ();
		Player player = Player.instance;
		Role playerRole = player.MyRole;
		CreateAgenda (playerRole.MyAgenda.items);
	}

	protected override void OnUpdateRoleEvent (UpdateRoleEvent e) {
		if (!Player.instance.IsDecider) {
			CreateAgenda ();
		}
	}
}
