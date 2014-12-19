using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent (typeof (NetworkView))]
public class AgendaItemsManager : MonoBehaviour {

	static public AgendaItemsManager instance;
	AgendaItem[] myItems = new AgendaItem[0];
	string playerName;
	List<AgendaItem> items = new List<AgendaItem> (0);			// All agenda items
	List<AgendaItem> votableItems = new List<AgendaItem> (0);	// Items that this player can vote on
	List<AgendaItem> winningItems = new List<AgendaItem> (0);	// Items that won the vote
	bool confirmed = false;
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
				if (item.playerName == playerName)
					myWinningItems.Add (item);
			}
			return myWinningItems;
		}
	}

	public int CurrentIndex {
		get { return index+1; }
	}

	public int TotalItems {
		get { return votableItems.Count; }
	}

	public AgendaItem NextItem {
		get {
			if (index >= votableItems.Count)
				return null;
			if (index == 0)
				PrepareItems ();
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
		Events.instance.AddListener<HostSendMessageEvent> (OnHostSendMessageEvent);
		Events.instance.AddListener<SelectDeciderEvent> (OnSelectDeciderEvent);
		Events.instance.AddListener<DeciderReceiveMessageEvent> (OnDeciderReceiveMessageEvent);
	}

	public void PopulateAgendaItems () {
		if (MultiplayerManager.instance.Hosting)
			Events.instance.Raise (new HostScheduleMessageEvent ("SendAgendaItems"));
	}

	public AgendaItem GetAgendaItem (string playerName, string description) {
		foreach (AgendaItem item in items) {
			if (item.playerName == playerName && item.description == description)
				return item;
		}
		return null;
	}

	public void AddVote (AgendaItem item, bool isDecider=false) {
		item.AddVote (isDecider);
	}

	void OnHostSendMessageEvent (HostSendMessageEvent e) {
		if (e.message == "SendAgendaItems") {
			networkView.RPC ("SendAgendaItems", RPCMode.All);
		}
	}

	void SendAgendaItem (AgendaItem item) {
		networkView.RPC ("ReceiveAgendaItem", RPCMode.All, item.playerName, item.description, item.bonus);
	}

	void PrepareItems () {
		if (!Player.instance.IsDecider)
			RemovePlayerItems (playerName);
		votableItems = votableItems.Shuffle ();
	}

	void CopyItemsToVotable () {
		votableItems = new List<AgendaItem>();
		for (int i = 0; i < items.Count; i ++) {
			votableItems.Add (items[i]);
		}
	}

	void RemovePlayerItems (string name) {
		List<AgendaItem> tempItems = new List<AgendaItem>();
		foreach (AgendaItem item in votableItems) {
			if (item.playerName != name)
				tempItems.Add (item);
		}
		votableItems = tempItems;
	}

	void OnSelectDeciderEvent (SelectDeciderEvent e) {
		RemovePlayerItems (e.name);
		playerCount = MultiplayerManager.instance.PlayerCount;
	}

	void OnDeciderReceiveMessageEvent (DeciderReceiveMessageEvent e) {
		if (e.id == "AddVote")
			AddVote (GetAgendaItem (e.message1, e.message2));
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
				if (votableItems[i].VoteCount > majority) {
					won = true;
				}
				if (votableItems[i].VoteCount == majority && votableItems[i].DeciderVote) {
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
		MessageRelayer.instance.SendMessageToAll ("FinishReceivingWins");
	}

	[RPC]
	void ReceiveAgendaItem (string otherName, string description, int bonus) {
		
		if (!confirmed) {
			Events.instance.Raise (new ClientConfirmMessageEvent ("SendAgendaItem"));
			confirmed = true;
		}

		AgendaItem ai = new AgendaItem (otherName, description, bonus);
		items.Add (ai);
		votableItems.Add (ai);
	}

	[RPC]
	void SendAgendaItems () {
		playerName = Player.instance.Name;
		myItems = Player.instance.MyRole.MyAgenda.items;
		for (int i = 0; i < myItems.Length; i ++) {
			SendAgendaItem (myItems[i]);
		}
	}

	[RPC]
	void ReceiveWinningAgendaItem (string name, string description) {
		AgendaItem i = GetAgendaItem (name, description);
		if (i != null)
			winningItems.Add (i);
	}
}
