using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent (typeof (NetworkView))]
public class RoleManager : MonoBehaviour {

	Deck deck;
	static public RoleManager instance;

	List<string> playerNames;
	int[] randomIndices;

	void Awake () {
		if (instance == null)
			instance = this;
		Events.instance.AddListener<HostSendMessageEvent> (OnHostSendMessageEvent);
	}

	public void PopulateDeck (Deck deck) {
		this.deck = deck;
		SetRandomRoles ();
	}

	void SetRandomRoles () {

		// The host assigns random roles to the rest of the players
		if (!MultiplayerManager.instance.Hosting)
			return;
		
		playerNames = MultiplayerManager.instance.Players;
		randomIndices = new int[playerNames.Count];
		int rolesCount = deck.Roles.Length;
		List<int> roleIndices = new List<int>();
		
		// Generate a list of numbers iterating by 1
		for (int i = 0; i < rolesCount; i ++) {
			roleIndices.Add (i);
		}
		
		// Randomly assign the numbers to an array
		for (int i = 0; i < randomIndices.Length; i ++) {
			
			if (roleIndices.Count == 0) {
				Debug.LogWarning ("Not enough roles in the deck to give everyone a different role.");
				randomIndices[i] = 0;
				continue;
			}

			int r = Random.Range (0, roleIndices.Count-1);
			randomIndices[i] = roleIndices[r];
			roleIndices.Remove (roleIndices[r]);
		}

		Events.instance.Raise (new HostScheduleMessageEvent ("HostAssignRoles"));
	}

	void HostAssignRoles () {

		// Each player is sent a message with a name and number. 
		// If their name matches the messaged name, they're assigned the corresponding role
		for (int i = 0; i < playerNames.Count; i ++) {
			networkView.RPC ("AssignRole", RPCMode.All, playerNames[i], randomIndices[i]);
		}
	}

	void OnHostSendMessageEvent (HostSendMessageEvent e) {
		if (e.message == "HostAssignRoles") {
			HostAssignRoles ();
		}
	}

	[RPC]
	void AssignRole (string name, int roleIndex) {
		if (name == Player.instance.Name) {
			Debug.Log (deck.Name);
			Debug.Log (deck.Roles[roleIndex].name);
			Events.instance.Raise (new SetRoleEvent (deck.Roles[roleIndex]));
		}
	}

	/**
	 *	Debugging
	 */

	public void DebugAssignRole () {
		Role role = deck.Roles[Random.Range (0, deck.Roles.Length-1)];
		Events.instance.Raise (new SetRoleEvent (role));		
	}
}
