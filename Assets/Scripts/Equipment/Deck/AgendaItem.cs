using UnityEngine;
using System.Collections;

public class AgendaItem : System.Object {

	public readonly string playerName;
	public readonly string description;
	public readonly int bonus;

	int voteCount = 0;
	public int VoteCount {
		get { return voteCount; }
	}

	bool deciderVote = false;
	public bool DeciderVote {
		get { return deciderVote; }
	}

	bool won = false;
	public bool Won {
		get { return won; }
		set { won = value; }
	}
	
	public AgendaItem (string playerName, string description, int bonus) {
		this.playerName = playerName;
		this.description = description;
		this.bonus = bonus;
		Events.instance.AddListener<RoundStartEvent> (OnRoundStartEvent);
	}

	public void AddVote (bool isDecider=false) {
		voteCount ++;
		if (isDecider)
			deciderVote = true;
	}

	void OnRoundStartEvent (RoundStartEvent e) {
		voteCount = 0;
	}
}
