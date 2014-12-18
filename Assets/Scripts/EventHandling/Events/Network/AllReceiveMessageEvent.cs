using UnityEngine;
using System.Collections;

public class AllReceiveMessageEvent : GameEvent {

	public readonly string id;
	public readonly string message1;
	public readonly string message2;

	public AllReceiveMessageEvent (string id, string message1, string message2) {
		this.id = id;
		this.message1 = message1;
		this.message2 = message2;
	}
}
