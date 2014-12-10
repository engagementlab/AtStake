using UnityEngine;
using System.Collections;

public class RoleScreen : GameScreen {

	Role playerRole = null;
	string playerName = "";

	public RoleScreen (GameState state, string name = "Role") : base (state, name) {
		
	}

	protected override void OnScreenStartPlayer () {
		
		if (playerRole != null) 
			return;
		
		Player player = Player.instance;
		playerRole = player.MyRole;
		playerName = player.Name;

		string title = string.Format ("{0} the {1}", playerName, playerRole.name);
		string bio = playerRole.bio;
		AgendaItem[] items = playerRole.MyAgenda.items;
		
		ScreenElement[] se = new ScreenElement[items.Length*2 + 5];
		
		se[0] = new LabelElement (title);
		se[1] = new LabelElement (bio);
		se[2] = new LabelElement ("Agenda");
		
		int index = 0;
		for (int i = 3; i < se.Length-2; i += 2) {
			AgendaItem item = items[index];
			se[i] = new LabelElement (item.description);
			se[i+1] = new LabelElement (string.Format ("Bonus: +{0} points", item.bonus));
			index ++;
		}

		string beanCount = string.Format ("Coins: {0}", player.MyBeanPool.BeanCount);
		se[se.Length-2] = new LabelElement (beanCount);
		se[se.Length-1] = CreateButton ("Back");

		SetVariableElements (se);
	}

	protected override void OnButtonPress (ButtonPressEvent e) {
		if (e.id == "Back") {
			GameStateController.instance.GotoPreviouslyVisitedScreen ();
		}
	}
}
