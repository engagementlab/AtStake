using UnityEngine;
using System.Collections;

public class RoundStartManager : MonoBehaviour {

	void Awake () {
		Events.instance.AddListener<RoundStartEvent> (OnRoundStartEvent);
		Events.instance.AddListener<HostSendMessageEvent> (OnHostSendMessageEvent);
		Events.instance.AddListener<AllReceiveMessageEvent> (OnAllReceiveMessageEvent);
	}

	void OnRoundStartEvent (RoundStartEvent e) {
		Debug.Log ("round started. decider ? " + Player.instance.IsDecider + ". hosting ? " + MultiplayerManager.instance.Hosting);
		AgendaItemsManager.instance.ClearItems ();
		if (MultiplayerManager.instance.Hosting) {
			BeanPotManager.instance.OnRoundStart ();
			RoleManager.instance.SetRandomRoles ();
			MessageSender.instance.ScheduleMessage ("UpdateAgendaItems");
			MessageSender.instance.ScheduleMessage ("SendVotableItems");
		}
	}

	void OnHostSendMessageEvent (HostSendMessageEvent e) {
		if (e.name == "UpdateAgendaItems") {
			MessageSender.instance.SendMessageToAll ("UpdateAgendaItems");
		} else if (e.name == "SendVotableItems") {
			MessageSender.instance.SendMessageToAll ("SendVotableItems");
		}
	}

	void OnAllReceiveMessageEvent (AllReceiveMessageEvent e) {
		if (e.id == "UpdateAgendaItems") {
			UpdateAgendaItems ();
		} else if (e.id == "SendVotableItems") {
			SendVotableItems ();
		}
	}

	void UpdateAgendaItems () {
		AgendaItemsManager.instance.UpdateMyItems ();
		Events.instance.Raise (new UpdateRoleEvent ());
	}

	void SendVotableItems () {
		AgendaItemsManager.instance.SendVotableItems ();
	}
}
