using UnityEngine;
using System.Collections;

public class RoleScreen : GameScreen {

	string playerName = "";

	public RoleScreen (GameState state, string name = "Role") : base (state, name) {
		Events.instance.AddListener<UpdateRoleEvent> (OnUpdateRoleEvent);
	}

	//public override void OnScreenStart (bool hosting, bool isDecider) {}

	protected void CreateRoleCard () {
		
		Player player = Player.instance;
		Role playerRole = player.MyRole;
		playerName = player.Name;

		//AppendVariableElements (RoleDescription (playerName, playerRole.name, playerRole.bio));
		//AppendVariableElements (RoleAgendaItems (playerRole.MyAgenda.items));

		CreateBeans ();
		CreateBio (playerName, playerRole.name, playerRole.bio);
		CreateAgenda (playerRole.MyAgenda.items);

		ScreenElements.AddEnabled ("back", CreateBottomButton ("Back"));
	}

	protected void CreateBeans () {
		ScreenElements.AddEnabled ("pool", new BeanPoolElement ());
		ScreenElements.AddEnabled ("pot", new BeanPotElement ());
	}

	protected void CreateBio (string playerName, string playerRole, string bio) {
		string title = string.Format ("{0} the {1}", playerName, playerRole);
		ScreenElements.SuspendUpdating ();
		ScreenElements.Add<LabelElement> ("title", new LabelElement (title, 0)).Content = title;
		ScreenElements.Add<LabelElement> ("bio", new LabelElement (bio, 1)).Content = bio;
		ScreenElements.EnableUpdating ();
	}

	protected void CreateAgenda (AgendaItem[] items) {
		ScreenElements.SuspendUpdating ();
		ScreenElements.AddEnabled ("agendaTitle", new LabelElement ("Agenda", 2, new DefaultCenterTextStyle ()));
		int index = 0;
		int position = 3;
		for (int i = 0; i < items.Length; i ++) {
			
			AgendaItem item = items[index];
			
			string descriptionID = "description" + i.ToString ();
			string description = item.description;
			string bonusID = "bonus" + i.ToString ();
			string bonus = string.Format ("Bonus: +{0} points", item.bonus);

			ScreenElements.Add<LabelElement> (descriptionID, new LabelElement (description, position, new SmallTextStyle ())).Content = description;
			position ++;
			ScreenElements.Add<LabelElement> (bonusID, new LabelElement (bonus, position, new BonusTextStyle ())).Content = bonus;
			position ++;
			index ++;
		}
		ScreenElements.EnableUpdating ();
	}

	/*protected ScreenElement[] RoleDescription (string playerName, string playerRole, string bio) {
		string title = string.Format ("{0} the {1}", playerName, playerRole);
		return new ScreenElement[] {
			new BeanPoolElement (),
			new BeanPotElement (),
			new LabelElement (title, 0),
			new LabelElement (bio, 1)
		};
	}

	protected ScreenElement[] RoleAgendaItems (AgendaItem[] items) {
		ScreenElement[] se = new ScreenElement[items.Length*2+1];
		int index = 0;
		se[0] = new LabelElement ("Agenda", 2, new DefaultCenterTextStyle ());
		for (int i = 1; i < se.Length; i += 2) {
			AgendaItem item = items[index];
			se[i] = new LabelElement (item.description, i+2, new SmallTextStyle ());
			se[i+1] = new LabelElement (string.Format ("Bonus: +{0} points", item.bonus), i+3, new BonusTextStyle ());
			index ++;
		}
		return se;
	}

	protected virtual void AddBackButton () {
		AppendVariableElements (CreateBottomButton ("Back"));
	}*/

	protected override void OnButtonPress (ButtonPressEvent e) {
		if (e.id == "Back") {
			GameStateController.instance.GotoPreviouslyVisitedScreen ();
		}
	}

	protected virtual void OnUpdateRoleEvent (UpdateRoleEvent e) {
		/*if (!Player.instance.IsDecider) {
			ClearScreen ();
			CreateRoleCard ();
			AddBackButton ();
		}*/
		if (Player.instance.IsDecider) {
			ScreenElements.Disable ("back");
		} else {
			CreateRoleCard ();
		}
	}
}
