using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent (typeof (NetworkView))]
public class AgendaItemsManager : MonoBehaviour {

	static public AgendaItemsManager instance;
	AgendaItem[] myItems = new AgendaItem[0];
	string playerName;
	List<AgendaItem> items = new List<AgendaItem> (0);
	List<AgendaItem> winningItems = new List<AgendaItem> (0);
	bool confirmed = false;
	int index = 0;
	int finishedVoters = 0;
	int playerCount = 0;

	public List<AgendaItem> WinningItems {
		get { return winningItems; }
	}

	public int CurrentIndex {
		get { return index+1; }
	}

	public int TotalItems {
		get { return items.Count; }
	}

	public AgendaItem NextItem {
		get {
			if (index >= items.Count)
				return null;
			if (index == 0)
				PrepareItems ();
			AgendaItem nextItem = items[index];
			index ++;
			return nextItem;
		}
	}

	public bool HasNextItem {
		get { return index != items.Count; }
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
		items = items.Shuffle ();
	}

	void RemovePlayerItems (string name) {
		List<AgendaItem> tempItems = new List<AgendaItem>();
		foreach (AgendaItem item in items) {
			if (item.playerName != name)
				tempItems.Add (item);
		}
		items = tempItems;
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
		bool even = playerCount % 2 == 0;
		for (int i = 0; i < items.Count; i ++) {
			bool won = false;
			if (even) {
				int majority = playerCount/2;
				if (items[i].VoteCount > majority) {
					won = true;
				}
				if (items[i].VoteCount == majority && items[i].DeciderVote) {
					won = true;
				}
			} else {
				if (items[i].VoteCount >= Mathf.Ceil (playerCount / 2)) {
					won = true;
				}
			}
			if (won) {
				networkView.RPC ("ReceiveWinningAgendaItem", RPCMode.All, items[i].playerName, items[i].description);
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

		items.Add (new AgendaItem (otherName, description, bonus));
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
		winningItems.Add (GetAgendaItem (name, description));
	}
}
