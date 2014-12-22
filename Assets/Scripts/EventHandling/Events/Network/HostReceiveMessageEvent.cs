using UnityEngine;
using System.Collections;

public class HostReceiveMessageEvent : GameEvent {

	public readonly string id;
	public readonly string message1;
	public readonly string message2;

	public HostReceiveMessageEvent (string id, string message1, string message2) {
		this.id = id;
		this.message1 = message1;
		this.message2 = message2;
	}
}
