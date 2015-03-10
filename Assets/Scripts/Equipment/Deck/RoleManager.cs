using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RoleManager : MonoBehaviour {

	Deck deck;
	static public RoleManager instance;

	List<string> playerNames;
	int[] randomIndices;

	void Awake () {
		if (instance == null)
			instance = this;
		Events.instance.AddListener<AllReceiveMessageEvent> (OnAllReceiveMessageEvent);
	}

	public void PopulateDeck (Deck deck) {
		this.deck = deck;
	}

	public void SetRandomRoles () {

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

			int r = Random.Range (0, roleIndices.Count);
			randomIndices[i] = roleIndices[r];
			roleIndices.Remove (roleIndices[r]);
		}

		HostAssignRoles ();
	}

	void HostAssignRoles () {

		// Each player is sent a message with a name and number. 
		// If their name matches the messaged name, they're assigned the corresponding role
		for (int i = 0; i < playerNames.Count; i ++) {
			MessageSender.instance.SendMessageToAll ("AssignRole", playerNames[i], "", randomIndices[i]);
		}
	}

	void OnAllReceiveMessageEvent (AllReceiveMessageEvent e) {
		if (e.id == "AssignRole") {
			if (e.message1 == Player.instance.Name) {
				Events.instance.Raise (new SetRoleEvent (deck.Roles[e.val]));
			}
		}
	}
}
