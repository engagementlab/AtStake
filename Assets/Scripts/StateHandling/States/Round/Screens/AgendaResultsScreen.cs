using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AgendaResultsScreen : GameScreen {

	LabelElement description;

	public AgendaResultsScreen (GameState state, string name = "Agenda Results") : base (state, name) {
		Events.instance.AddListener<AllReceiveMessageEvent> (OnAllReceiveMessageEvent);
		description = new LabelElement ("please wait while everyone finishes voting :)");
		SetStaticElements (new ScreenElement[] {
			description
		});
	}

	public override void OnScreenStart (bool isHosting, bool isDecider) {
		MessageRelayer.instance.SendMessageToDecider ("FinishedVoting");
	}

	void OnAllReceiveMessageEvent (AllReceiveMessageEvent e) {
		List<AgendaItem> winningItems = AgendaItemsManager.instance.WinningItems;
		ScreenElement[] se = new ScreenElement[winningItems.Count];
		for (int i = 0; i < se.Length; i ++) {
			se[i] = new LabelElement (string.Format ("{0}: {1} +{2} points", winningItems[i].playerName, winningItems[i].description, winningItems[i].bonus));
		}
	}
}
