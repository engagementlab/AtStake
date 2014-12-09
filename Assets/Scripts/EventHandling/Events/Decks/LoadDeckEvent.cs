using UnityEngine;
using System.Collections;

public class LoadDeckEvent : GameEvent {

	public readonly Deck deck;

	public LoadDeckEvent (Deck deck) {
		this.deck = deck;
	}
}
