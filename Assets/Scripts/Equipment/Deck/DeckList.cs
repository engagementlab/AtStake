using UnityEngine;
using System;
using System.Collections.Generic;

// Used for displaying a list of playable decks,
// does not contain any information about the decks as far as gameplay is concerned
public class DeckList {

	List<DeckItem> localDecks = new List<DeckItem>();
	List<DeckItem> hostedDecks = new List<DeckItem>();

	public List<DeckItem> LocalDecks {
		get { return localDecks; }
	}

	public List<DeckItem> HostedDecks {
		get { return hostedDecks; }
	}

	public DeckList () {}

	public void ClearLocal () {
		localDecks.Clear ();
	}

	public void ClearHosted () {
		hostedDecks.Clear ();
	}

	public void AddLocalDeck (string name, string filename, string starred) {
		localDecks.Add (new DeckItem (name, filename, starred == "true"));
	}

	public void AddHostedDeck (string name, string filename, string starred) {
		hostedDecks.Add (new DeckItem (name, filename, starred == "true"));	
	}

	public void Updated () {
		Events.instance.Raise (new UpdateDeckListEvent (this));
	}
}

public class DeckItem {

	public readonly string name;
	public readonly string filename;
	public readonly bool starred;

	public DeckItem (string name, string filename, bool starred) {
		this.name = name;
		this.filename = filename;
		this.starred = starred;
	}
}