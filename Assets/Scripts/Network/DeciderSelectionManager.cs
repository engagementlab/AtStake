using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent (typeof(NetworkView))]
public class DeciderSelectionManager : MonoBehaviour {

	// if the player is hosting, keeps track of clients' selection for decider
	// after all players have have submitted a selection and all selections match, the game begins

	public static DeciderSelectionManager instance;
	List<string[]> selections = new List<string[]> ();

	void Start () {
		if (instance == null) instance = this;
	}

	public void SelectDecider (string deciderName) {
		string name = MultiplayerManager.instance.PlayerName;
		if (MultiplayerManager.instance.Hosting) {
			AddSelection (name, deciderName);
		} else {
			networkView.RPC ("AddSelection", RPCMode.Server, name, deciderName);
		}
	}

	bool SelectionsMatch () {
		string firstSelection = "";
		foreach (string[] s in selections) {
			if (firstSelection == "") {
				firstSelection = s[1];
			} else {
				if (s[1] != firstSelection)
					return false;
			}
		}
		return true;
	}

	[RPC]
	void AddSelection (string name, string deciderName) {

		bool nameInList = false;
		foreach (string[] s in selections) {
			if (s[0] == name) {
				s[1] = deciderName;
				nameInList = true;
			}
		}
		if (!nameInList)
			selections.Add (new string[] { name, deciderName });

		if (selections.Count == MultiplayerManager.instance.PlayerCount && SelectionsMatch ()) {
			GameStateController.instance.AllPlayersGotoScreen ("Question", "Round");
		}
	}
}
