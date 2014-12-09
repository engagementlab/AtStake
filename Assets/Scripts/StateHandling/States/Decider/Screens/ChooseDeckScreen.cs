using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ChooseDeckScreen : GameScreen {
	
	LabelElement title;
	DeckList dl;

	public ChooseDeckScreen (GameState state, string name = "Choose Deck") : base (state, name) {
		Events.instance.AddListener<UpdateDeckListEvent> (OnUpdateDeckListEvent);
		title = new LabelElement ("Please wait while the host chooses a deck");
		SetStaticElements (new ScreenElement[] {
			title	
		});
	}
	
	public override void OnScreenStartHost () {

		title.content = "Choose a deck";

		int localDecksCount = dl.LocalDecks.Count;
		int hostedDecksCount = dl.HostedDecks.Count;
		int elementsCount = localDecksCount + hostedDecksCount+2;
		ScreenElement[] se = new ScreenElement[elementsCount];

		se[0] = new LabelElement ("Local decks");
		int offset = 1;
		for (int i = offset; i < localDecksCount+offset; i ++) {
			string name = dl.LocalDecks[i-offset].name;
			se[i] = CreateButton ("deck_l_" + name, name);
		}

		se[localDecksCount+1] = new LabelElement ("Hosted decks");
		offset = localDecksCount+2;
		for (int i = offset; i < elementsCount; i ++) {
			string name = dl.HostedDecks[i-offset].name;
			se[i] = CreateButton ("deck_h_" + name, name);
		}
		SetVariableElements (se);
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

	public override void OnButtonPress (ButtonPressEvent e) {
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
