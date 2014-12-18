using UnityEngine;
using System.Collections;

public class AgendaScreen : GameScreen {
	
	LabelElement progress;
	LabelElement description;
	ButtonElement yesButton;
	ButtonElement noButton;
	AgendaItem currentItem;

	public AgendaScreen (GameState state, string name = "Agenda") : base (state, name) {

		progress = new LabelElement ();
		description = new LabelElement ();
		yesButton = CreateButton ("yah");
		noButton = CreateButton ("nah");

		SetStaticElements (new ScreenElement[] {
			description,
			yesButton,
			noButton
		});
	}

	public override void OnScreenStart (bool hosting, bool isDecider) {
		AgendaItemsManager aiManager = AgendaItemsManager.instance;
		currentItem = aiManager.NextItem;
		if (currentItem == null) {
			GotoScreen ("Agenda Results");
			return;
		}
		progress.content = string.Format ("{0} of {1} items", aiManager.CurrentIndex, aiManager.TotalItems);
		description.content = string.Format ("Vote yes if you think {0}'s proposal {1}", Player.instance.WinningPlayer, currentItem.description);
	}

	protected override void OnButtonPress (ButtonPressEvent e) {
		switch (e.id) {
			case "yah": AddVote (); break;
			case "nah": break;
		}
		HandleGotoScreen ();
	}

	void HandleGotoScreen () {
		if (AgendaItemsManager.instance.HasNextItem) {
			RefreshScreen ();
		} else {
			GotoScreen ("Agenda Results");
		}
	}

	void AddVote () {
		if (Player.instance.IsDecider) {
			AgendaItemsManager.instance.AddVote (currentItem, true);
		} else {
			MessageRelayer.instance.SendMessageToDecider ("AddVote", currentItem.playerName, currentItem.description);
		}
	}
}
