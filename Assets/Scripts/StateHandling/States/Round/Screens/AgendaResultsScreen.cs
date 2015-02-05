using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AgendaResultsScreen : GameScreen {

	LabelElement description;
	string defaultDescription = "please wait while everyone finishes voting :)";

	public AgendaResultsScreen (GameState state, string name = "Agenda Results") : base (state, name) {
		
		Events.instance.AddListener<AllReceiveMessageEvent> (OnAllReceiveMessageEvent);
		Events.instance.AddListener<RoundStartEvent> (OnRoundStartEvent);
		
		description = new LabelElement (defaultDescription, 0);
		ScreenElements.AddEnabled ("description", description);
		ScreenElements.AddEnabled ("wait", new ImageElement ("wait", 1, Color.white));
		ScreenElements.AddDisabled ("next", CreateBottomButton ("Next", "", "bottomPink", Side.Right));
	}

	public override void OnScreenStart (bool isHosting, bool isDecider) {
		MessageRelayer.instance.SendMessageToDecider ("FinishedVoting");
	}

	void OnAllReceiveMessageEvent (AllReceiveMessageEvent e) {
		
		if (e.id != "FinishReceivingWins") {
			return;
		}

		Player player = Player.instance;
		ScreenElements.SuspendUpdating ();
		ScreenElements.DisableAll ();
		ScreenElements.Enable ("description");
		ScreenElements.Enable ("next");
		description.Content = "Winning Agenda Items:";
		List<AgendaItem> winningItems = AgendaItemsManager.instance.WinningItems;

		for (int i = 0; i < winningItems.Count; i ++) {
			string item = string.Format ("{0}: {1} +{2} points", winningItems[i].playerName, winningItems[i].description, winningItems[i].bonus);
			ScreenElements.Add<LabelElement> ("item" + i.ToString(), new LabelElement (item, i+1)).Content = item;
		}

		ScreenElements.EnableUpdating ();

		// Update score
		List<AgendaItem> myWinningItems = AgendaItemsManager.instance.MyWinningItems;
		foreach (AgendaItem item in myWinningItems) {
			player.MyBeanPool.OnAddBonus (item.bonus);
		}
		BeanPoolManager.instance.UpdateMyScore ();
	}

	protected override void OnButtonPress (ButtonPressEvent e) {
		if (e.id == "Next") {
			RoundState round = state as RoundState;
			if (round.RoundNumber < 3) {
				GotoScreen ("Scoreboard");
			} else {
				GotoScreen ("Final Scoreboard", "End");
			}
		}
	}

	void OnRoundStartEvent (RoundStartEvent e) {
		ScreenElements.SuspendUpdating ();
		ScreenElements.DisableAll ();
		ScreenElements.Enable ("description");
		ScreenElements.Enable ("wait");
		description.Content = defaultDescription;
	}
}
