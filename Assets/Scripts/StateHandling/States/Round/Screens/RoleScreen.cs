using UnityEngine;
using System.Collections;

public class RoleScreen : GameScreen {

	string playerName = "";

	public RoleScreen (GameState state, string name = "Role") : base (state, name) {
		Events.instance.AddListener<UpdateRoleEvent> (OnUpdateRoleEvent);
	}

	public void OnStartScreen (bool hosting, bool isDecider) {}

	protected void CreateRoleCard () {
		
		Player player = Player.instance;
		Role playerRole = player.MyRole;
		playerName = player.Name;

		AppendVariableElements (RoleDescription (playerName, playerRole.name, playerRole.bio));
		AppendVariableElements (RoleAgendaItems (playerRole.MyAgenda.items));
		AppendVariableElements (RoleBeans (player.MyBeanPool.BeanCount));
	}

	ScreenElement[] RoleDescription (string playerName, string playerRole, string bio) {
		string title = string.Format ("{0} the {1}", playerName, playerRole);
		return new ScreenElement[3] {
			new LabelElement (title),
			new LabelElement (bio),
			new LabelElement ("Agenda")
		};
	}

	ScreenElement[] RoleAgendaItems (AgendaItem[] items) {
		ScreenElement[] se = new ScreenElement[items.Length*2];
		int index = 0;
		for (int i = 0; i < se.Length; i += 2) {
			AgendaItem item = items[index];
			se[i] = new LabelElement (item.description);
			se[i+1] = new LabelElement (string.Format ("Bonus: +{0} points", item.bonus));
			index ++;
		}
		return se;
	}

	ScreenElement[] RoleBeans (int beanCount) {
		string beanLabel = string.Format ("Coins: {0}", beanCount);
		return new ScreenElement[] {
			new LabelElement (beanLabel)
		};
	}

	protected virtual void AddBackButton () {
		AppendVariableElements (CreateButton ("Back"));
	}

	protected override void OnButtonPress (ButtonPressEvent e) {
		if (e.id == "Back") {
			GameStateController.instance.GotoPreviouslyVisitedScreen ();
		}
	}

	void OnUpdateRoleEvent (UpdateRoleEvent e) {
		if (!Player.instance.IsDecider) {
			ClearScreen ();
			CreateRoleCard ();
			AddBackButton ();
		}
	}
}
