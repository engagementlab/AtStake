using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent (typeof(NetworkView))]
public class DeciderSelectionManager : MonoBehaviour {

	public static DeciderSelectionManager instance;

	void Awake () {
		if (instance == null) 
			instance = this;
		Events.instance.AddListener<MessagesMatchEvent> (OnMessagesMatchEvent);
		Events.instance.AddListener<AllReceiveMessageEvent> (OnAllReceiveMessageEvent);
	}

	public void SetDecider (string deciderName) {
		MessageSender.instance.SendMessageToAll ("New Decider", deciderName);
	}

	public void SelectDecider (string deciderName) {
		MessageMatcher.instance.SetMessage ("SelectDecider", deciderName);
	}

	void OnMessagesMatchEvent (MessagesMatchEvent e) {
		if (e.id == "SelectDecider") {
			Events.instance.Raise (new SelectDeciderEvent (e.message));
			GameStateController.instance.GotoScreen ("Scoreboard", "Round");
		}
	}

	void OnAllReceiveMessageEvent (AllReceiveMessageEvent e) {
		if (e.id == "New Decider") {
			Events.instance.Raise (new SelectDeciderEvent (e.message1));
		}
	}
}
