using UnityEngine;
using System.Collections;

[System.Serializable]
public class DeciderManager : System.Object {

	[SerializeField] string deciderName = "";

	public bool IsDecider {
		get { return deciderName == Player.instance.Name; }
	}

	public DeciderManager () {
		Events.instance.AddListener<SelectDeciderEvent> (OnSelectDeciderEvent);
		Events.instance.AddListener<MessagesMatchEvent> (OnMessagesMatchEvent);
		Events.instance.AddListener<AllReceiveMessageEvent> (OnAllReceiveMessageEvent);
	}

	public void SetDecider (string deciderName) {
		MessageSender.instance.ScheduleMessage (
			new NetworkMessage ("New Decider", deciderName)
		);
	}

	public void SelectDecider (string deciderName) {
	 	MessageMatcher.instance.SetMessage ("SelectDecider", deciderName);
	}

	void OnSelectDeciderEvent (SelectDeciderEvent e) {
		deciderName = e.name;		
	}

	void OnAllReceiveMessageEvent (AllReceiveMessageEvent e) {
		if (e.id == "New Decider") {
			Events.instance.Raise (new SelectDeciderEvent (e.message1));
		}
	}

	void OnMessagesMatchEvent (MessagesMatchEvent e) {
		if (e.id == "SelectDecider") {
			Events.instance.Raise (new SelectDeciderEvent (e.message));
			GameScreenDirector.instance.DeciderSelected ();
			// GameStateController.instance.GotoScreen ("Scoreboard", "Round");
		}
	}
}

public enum FirstDecider {
	Host,
	Vote
}

public static class DeciderSelectionStyle {
	public static FirstDecider FirstDecider = FirstDecider.Host;
	public static bool Host { get { return FirstDecider == FirstDecider.Host; } }
	public static bool Vote { get { return FirstDecider == FirstDecider.Vote; } }
}