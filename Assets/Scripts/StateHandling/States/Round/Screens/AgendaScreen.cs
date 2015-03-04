using UnityEngine;
using System.Collections;

public class AgendaScreen : GameScreen {
	
	LabelElement progress;
	LabelElement description;
	ButtonElement yesButton;
	ButtonElement noButton;
	AgendaItem currentItem;

	public AgendaScreen (GameState state, string name = "Agenda Item") : base (state, name) {

		progress = new LabelElement ("", 0);
		description = new LabelElement ("", 1);
		yesButton = CreateButton ("Yes", 2);
		noButton = CreateButton ("No", 3);

		ScreenElements.AddEnabled ("progress", progress);
		ScreenElements.AddEnabled ("description", description);
		ScreenElements.AddEnabled ("yes", yesButton);
		ScreenElements.AddEnabled ("no", noButton);
	}

	public override void OnScreenStart (bool hosting, bool isDecider) {
		AgendaItemsManager aiManager = AgendaItemsManager.instance;
		currentItem = aiManager.NextItem;
		if (currentItem == null) {
			GotoScreen ("Agenda Results");
			return;
		}
		progress.Content = string.Format ("{0} / {1}", aiManager.CurrentIndex, aiManager.TotalItems);
		description.Content = Copy.AgendaItemVote (Player.instance.WinningPlayer, currentItem.description); //string.Format ("Vote yes if you think {0}'s proposal {1}", Player.instance.WinningPlayer, currentItem.description);
	}

	protected override void OnButtonPress (ButtonPressEvent e) {
		switch (e.id) {
			case "Yes": AddVote (); break;
			case "No": break;
		}
		HandleGotoScreen ();
	}

	void HandleGotoScreen () {
		if (AgendaItemsManager.instance.HasNextItem) {
			RefreshScreen ();
		} else {
			if (AgendaVotingType.All) {
				GotoScreen ("Agenda Results");
			} else if (AgendaVotingType.Decider) {
				GameStateController.instance.AllPlayersGotoScreen ("Agenda Results");
				AgendaItemsManager.instance.CalculateDeciderVotes ();
			}
		}
	}

	void AddVote () {
		if (Player.instance.IsDecider) {
			AgendaItemsManager.instance.AddVote (currentItem, true);
		} else {
			//MessageRelayer.instance.SendMessageToDecider ("AddVote", currentItem.playerName, currentItem.description);
			MessageSender.instance.SendMessageToDecider ("AddVote", currentItem.playerName, currentItem.description);
		}
	}
}
