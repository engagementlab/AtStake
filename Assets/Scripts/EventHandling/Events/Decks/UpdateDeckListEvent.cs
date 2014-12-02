using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UpdateDeckListEvent : GameEvent {

	public DeckList deckList;

	public UpdateDeckListEvent (DeckList deckList) {
		this.deckList = deckList;
	}
}
