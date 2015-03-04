using UnityEngine;
using System.Collections;

[RequireComponent (typeof (NetworkView))]
public class RoundStartManager : MonoBehaviour {

	void Awake () {
		Events.instance.AddListener<RoundStartEvent> (OnRoundStartEvent);
		Events.instance.AddListener<HostSendMessageEvent> (OnHostSendMessageEvent);
	}

	void OnRoundStartEvent (RoundStartEvent e) {
		AgendaItemsManager.instance.ClearItems ();
		if (MultiplayerManager2.instance.Hosting) {
			BeanPotManager.instance.OnRoundStart ();
			RoleManager.instance.SetRandomRoles ();
			MessageSender.instance.ScheduleMessage ("UpdateAgendaItems");
			MessageSender.instance.ScheduleMessage ("SendVotableItems");
		}
	}

	void OnHostSendMessageEvent (HostSendMessageEvent e) {
		if (e.name == "UpdateAgendaItems") {
			networkView.RPC ("UpdateAgendaItems", RPCMode.All);
		}
		if (e.name == "SendVotableItems") {
			networkView.RPC ("SendVotableItems", RPCMode.All);
		}
	}

	[RPC]
	void UpdateAgendaItems () {
		AgendaItemsManager.instance.UpdateMyItems ();
		Events.instance.Raise (new UpdateRoleEvent ());
	}

	[RPC]
	void SendVotableItems () {
		AgendaItemsManager.instance.SendVotableItems ();
	}
}
