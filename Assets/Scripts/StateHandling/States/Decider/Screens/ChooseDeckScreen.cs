using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ChooseDeckScreen : GameScreen {
	
	LabelElement title;
	DeckList dl;

	public ChooseDeckScreen (GameState state, string name = "Choose Deck") : base (state, name) {
		Events.instance.AddListener<UpdateDeckListEvent> (OnUpdateDeckListEvent);
		title = new LabelElement (Copy.ChooseDeckClient, 0);
		ScreenElements.AddEnabled ("title", title);
	}
	
	protected override void OnScreenStartHost () {

		ScreenElements.SuspendUpdating ();
		title.Content = Copy.ChooseDeckHost;

		int localDecksCount = dl.LocalDecks.Count;
		int hostedDecksCount = dl.HostedDecks.Count;
		int elementsCount = localDecksCount + hostedDecksCount;

		for (int i = 0; i < localDecksCount; i ++) {
			string name = dl.LocalDecks[i].name;
			string id = "button" + i.ToString ();
			ScreenElements.Remove (id);
			ScreenElements.AddEnabled (id, CreateButton ("deck_l_" + name, i+2, name));
		}
		
		for (int i = localDecksCount; i < elementsCount; i ++) {
			string name = dl.HostedDecks[i-localDecksCount].name;
			string id = "button" + i.ToString ();
			ScreenElements.Remove (id);
			ScreenElements.AddEnabled (id, CreateButton ("deck_l_" + name, i+2, name));
		}
		ScreenElements.EnableUpdating ();
	}

	void OnUpdateDeckListEvent (UpdateDeckListEvent e) {
		dl = e.deckList;
	}

	string GetFilename (string name, bool local) {
		List<DeckItem> list = local ? dl.LocalDecks : dl.HostedDecks;
		foreach (DeckItem d in list) {
			if (d.name == name) {
				return d.filename;
			}
		}
		return "";
	}

	protected override void OnButtonPress (ButtonPressEvent e) {
		if (e.id.Length < 5)
			return;
		if (e.id.Substring (0, 5) == "deck_") {
			bool local = e.id.Substring (5, 2) == "l_";
			string name = e.id.Substring(7);
			string filename = GetFilename (name, local);
			DeckManager.instance.LoadDeck (filename, local);
		}
	}
}
