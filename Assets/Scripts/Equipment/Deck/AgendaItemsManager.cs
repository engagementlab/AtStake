using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent (typeof (NetworkView))]
public class AgendaItemsManager : MonoBehaviour {

	static public AgendaItemsManager instance;
	AgendaItem[] myItems = new AgendaItem[0];
	List<AgendaItem> items = new List<AgendaItem> (0);			// All agenda items in the deck
	List<AgendaItem> votableItems = new List<AgendaItem> (0);	// Items that this player can vote on
	List<AgendaItem> winningItems = new List<AgendaItem> (0);	// Items that won the vote
	int index = 0;
	int finishedVoters = 0;
	int playerCount = 0;

	public List<AgendaItem> WinningItems {
		get { return winningItems; }
	}

	public List<AgendaItem> MyWinningItems {
		get {
			List<AgendaItem> myWinningItems = new List<AgendaItem>();
			foreach (AgendaItem item in winningItems) {
				if (item.playerName == Player.instance.Name)
					myWinningItems.Add (item);
			}
			return myWinningItems;
		}
	}

	public int CurrentIndex {
		get { return index; }
	}

	public int TotalItems {
		get { return votableItems.Count; }
	}

	public AgendaItem NextItem {
		get {
			if (index >= votableItems.Count)
				return null;
			if (index == 0)
				ShuffleItems ();
			AgendaItem nextItem = votableItems[index];
			index ++;
			return nextItem;
		}
	}

	public bool HasNextItem {
		get { return index != votableItems.Count; }
	}

	void Awake () {
		if (instance == null)
			instance = this;
		Events.instance.AddListener<SelectDeciderEvent> (OnSelectDeciderEvent);
		Events.instance.AddListener<DeciderReceiveMessageEvent> (OnDeciderReceiveMessageEvent);
	}

	public void Populate (Deck deck) {
		items.Clear ();
		Role[] roles = deck.Roles;
		foreach (Role r in roles) {
			AgendaItem[] agendaItems = r.MyAgenda.items;
			foreach (AgendaItem ai in agendaItems) {
				items.Add (ai);
			}
		}
	}

	public void UpdateMyItems () {
		myItems = Player.instance.MyRole.MyAgenda.items;
	}

	public void SendVotableItems () {
		if (!Player.instance.IsDecider) {
			for (int i = 0; i < myItems.Length; i ++) {
				AgendaItem item = myItems[i];
				networkView.RPC ("ReceiveVotableItems", RPCMode.All, item.playerName, item.description, item.bonus);
			}
		}
	}

	public void ClearItems () {
		votableItems.Clear ();
		winningItems.Clear ();
		finishedVoters = 0;
		index = 0;
	}

	public void AddVote (AgendaItem item, bool isDecider=false) {
		item.AddVote (isDecider);
	}

	void ShuffleItems () {
		votableItems = votableItems.Shuffle ();
	}

	AgendaItem GetVotableItem (string playerName, string description) {
		foreach (AgendaItem item in votableItems) {
			if (item.playerName == playerName && item.description == description)
				return item;
		}
		return null;
	}

	AgendaItem GetItem (string description) {
		foreach (AgendaItem item in items) {
			if (item.description == description)
				return item;
		}
		return null;
	}

	public void CalculateDeciderVotes () {

		// Only count the Decider's votes
		for (int i = 0; i < votableItems.Count; i ++) {
			if (votableItems[i].VoteCount == 1) {
				networkView.RPC ("ReceiveWinningAgendaItem", RPCMode.All, votableItems[i].playerName, votableItems[i].description);
			}
		}
		MessageSender.instance.SendMessageToAll ("FinishReceivingWins");
	}

	void OnDeciderReceiveMessageEvent (DeciderReceiveMessageEvent e) {
		if (e.id == "AddVote")
			AddVote (GetVotableItem (e.message1, e.message2));
		if (e.id == "FinishedVoting") {
			finishedVoters ++;
			if (finishedVoters >= playerCount) {
				CalculateVotes ();
			}
		}
	}

	void CalculateVotes () {
		bool even = (playerCount-1) % 2 == 0;
		int majority = even ? (playerCount-1) / 2 : Mathf.CeilToInt (((float)playerCount-1f) / 2f);
		for (int i = 0; i < votableItems.Count; i ++) {
			bool won = false;
			if (even) {
				if (votableItems[i].VoteCount >= majority) {
					won = true;
				}
			} else {
				if (votableItems[i].VoteCount >= majority) {
					won = true;
				}
			}
			if (won) {
				networkView.RPC ("ReceiveWinningAgendaItem", RPCMode.All, votableItems[i].playerName, votableItems[i].description);
			}
		}
		MessageSender.instance.SendMessageToAll ("FinishReceivingWins");
	}

	void OnSelectDeciderEvent (SelectDeciderEvent e) {
		//playerCount = MultiplayerManager.instance.PlayerCount;
		playerCount = MultiplayerManager2.instance.PlayerCount;
	}

	[RPC]
	void ReceiveVotableItems (string playerName, string description, int bonus) {
		if (playerName != Player.instance.Name) {
			votableItems.Add (new AgendaItem (playerName, description, bonus));
		}
	}

	[RPC]
	void ReceiveWinningAgendaItem (string playerName, string description) {
		AgendaItem i;
		if (playerName == Player.instance.Name) {
			i = GetItem (description);
		} else {
			i = GetVotableItem (playerName, description);
		}
		winningItems.Add (i);
	}
}
