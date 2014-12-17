using UnityEngine;
using System.Collections;

public class DeciderReceiveMessageEvent : GameEvent {

	public readonly string message1;
	public readonly string message2;

	public DeciderReceiveMessageEvent (string message1, string message2) {
		this.message1 = message1;
		this.message2 = message2;
	}
}
