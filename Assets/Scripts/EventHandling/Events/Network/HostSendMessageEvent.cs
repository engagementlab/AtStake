using UnityEngine;
using System.Collections;

public class HostSendMessageEvent : GameEvent {

	public readonly string message;

	public HostSendMessageEvent (string message) {
		this.message = message;
	}
}
