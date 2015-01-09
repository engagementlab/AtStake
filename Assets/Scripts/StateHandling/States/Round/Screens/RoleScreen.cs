using UnityEngine;
using System.Collections;

public class RoleScreen : GameScreen {

	string playerName = "";

	public RoleScreen (GameState state, string name = "Role") : base (state, name) {
		Events.instance.AddListener<UpdateRoleEvent> (OnUpdateRoleEvent);
	}

	public override void OnScreenStart (bool hosting, bool isDecider) {}

	protected void CreateRoleCard () {
		
		Player player = Player.instance;
		Role playerRole = player.MyRole;
		playerName = player.Name;

		AppendVariableElements (RoleDescription (playerName, playerRole.name, playerRole.bio));
		AppendVariableElements (RoleAgendaItems (playerRole.MyAgenda.items));
		AppendVariableElements (RoleBeans (player.MyBeanPool.BeanCount));
	}

	protected ScreenElement[] RoleDescription (string playerName, string playerRole, string bio) {
		string title = string.Format ("{0} the {1}", playerName, playerRole);
		return new ScreenElement[2] {
			new LabelElement (title),
			new LabelElement (bio)
		};
	}

	protected ScreenElement[] RoleAgendaItems (AgendaItem[] items) {
		ScreenElement[] se = new ScreenElement[items.Length*2+1];
		int index = 0;
		se[0] = new LabelElement ("Agenda");
		for (int i = 1; i < se.Length; i += 2) {
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

	protected virtual void OnUpdateRoleEvent (UpdateRoleEvent e) {
		if (!Player.instance.IsDecider) {
			ClearScreen ();
			CreateRoleCard ();
			AddBackButton ();
		}
	}
}
