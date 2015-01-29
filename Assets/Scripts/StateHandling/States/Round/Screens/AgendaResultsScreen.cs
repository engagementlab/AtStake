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
		SetStaticElements (new ScreenElement[] {
			description
		});
	}

	public override void OnScreenStart (bool isHosting, bool isDecider) {
		SetVariableElements (new ScreenElement[] {
			new ImageElement ("wait", 1, Color.white)
		});
		MessageRelayer.instance.SendMessageToDecider ("FinishedVoting");
	}

	void OnAllReceiveMessageEvent (AllReceiveMessageEvent e) {
		
		if (e.id != "FinishReceivingWins") {
			return;
		}

		ClearScreen ();
		Player player = Player.instance;

		// Show the winning agenda items
		description.Content = "Winning Agenda Items:";
		List<AgendaItem> winningItems = AgendaItemsManager.instance.WinningItems;
		ScreenElement[] se = new ScreenElement[winningItems.Count];
		for (int i = 0; i < se.Length; i ++) {
			se[i] = new LabelElement (string.Format ("{0}: {1} +{2} points", winningItems[i].playerName, winningItems[i].description, winningItems[i].bonus), i+1);
		}

		SetVariableElements (se);
		AppendVariableElements (CreateBottomButton ("Next", "", "bottomPink", Side.Right));

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
		description.Content = defaultDescription;
		ClearScreen ();
	}
}
