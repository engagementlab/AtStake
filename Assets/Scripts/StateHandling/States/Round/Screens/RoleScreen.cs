using UnityEngine;
using System.Collections;

public class RoleScreen : GameScreen {

	string playerName = "";

	public RoleScreen (GameState state, string name = "Role") : base (state, name) {
		Events.instance.AddListener<UpdateRoleEvent> (OnUpdateRoleEvent);
		ScreenElements.AddEnabled ("back", CreateBottomButton ("Back"));
	}

	public override void OnScreenStart (bool hosting, bool isDecider) {
		base.OnScreenStart (hosting, isDecider);
		SetBackEnabled ();
	}

	protected virtual void SetBackEnabled () {
		if (Player.instance.IsDecider) {
			ScreenElements.Disable ("back");
		} else {
			ScreenElements.Enable ("back");
		}
	}

	protected void CreateRoleCard () {
		
		Player player = Player.instance;
		Role playerRole = player.MyRole;
		playerName = player.Name;

		CreateBeans ();
		CreateBio (playerName, playerRole.name, playerRole.bio);
		CreateAgenda (playerRole.MyAgenda.items);
	}

	protected void CreateBeans () {
		ScreenElements.AddEnabled ("pool", new BeanPoolElement ());
		ScreenElements.AddEnabled ("pot", new BeanPotElement ());
	}

	protected void CreateBio (string playerName, string playerRole, string bio) {
		string title = string.Format ("{0} the {1}", playerName, playerRole);
		ScreenElements.SuspendUpdating ();
		ScreenElements.Add<LabelElement> ("title", new LabelElement (title, 0, new DefaultCenterTextStyle ())).Content = title;
		ScreenElements.Add<LabelElement> ("bio", new LabelElement (bio, 1, new DefaultCenterTextStyle ())).Content = bio;
		ScreenElements.EnableUpdating ();
	}

	protected void CreateAgenda (AgendaItem[] items) {
		ScreenElements.SuspendUpdating ();
		ScreenElements.AddEnabled ("agendaTitle", new LabelElement ("Secret Agenda", 2, new DefaultCenterTextStyle ()));
		int index = 0;
		int position = 3;
		for (int i = 0; i < items.Length; i ++) {
			
			AgendaItem item = items[index];
			
			string descriptionID = "description" + i.ToString ();
			string description = item.description;
			string bonusID = "bonus" + i.ToString ();
			string bonus = string.Format ("Bonus: +{0} coins", item.bonus);

			ScreenElements.Add<LabelElement> (descriptionID, new LabelElement (description, position, new AgendaItemTextStyle ())).Content = description;
			position ++;
			ScreenElements.Add<LabelElement> (bonusID, new LabelElement (bonus, position, new BonusTextStyle ())).Content = bonus;
			position ++;
			index ++;
		}
		ScreenElements.EnableUpdating ();
	}

	protected override void OnButtonPress (ButtonPressEvent e) {
		if (e.id == "Back") {
			GoBackScreen (StageScreen.CurrentStage);
		}
	}

	protected virtual void OnUpdateRoleEvent (UpdateRoleEvent e) {
		if (!Player.instance.IsDecider) {
			CreateRoleCard ();
		}
	}
}
