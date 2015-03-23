using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DeciderSelectionManager : MonoBehaviour {

	public static DeciderSelectionManager instance;

	void Awake () {
		if (instance == null) 
			instance = this;
		Events.instance.AddListener<MessagesMatchEvent> (OnMessagesMatchEvent);
		Events.instance.AddListener<AllReceiveMessageEvent> (OnAllReceiveMessageEvent);
		Events.instance.AddListener<HostSendMessageEvent> (OnHostSendMessageEvent);
	}

	// First round
	public void SelectDecider (string deciderName) {
		MessageMatcher.instance.SetMessage ("SelectDecider", deciderName);
	}

	// Subsequent rounds
	public void SetDecider (string deciderName) {
		MessageSender.instance.ScheduleMessage (new NetworkMessage ("New Decider", deciderName));
	}

	void OnHostSendMessageEvent (HostSendMessageEvent e) {
		if (e.name == "New Decider") {
			MessageSender.instance.SendMessageToAll ("New Decider", e.message1);
		}
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
