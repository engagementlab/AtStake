using UnityEngine;
using System.Collections;

public class AllReceiveMessageEvent : GameEvent {

	public readonly string id;
	public readonly string message1;
	public readonly string message2;
	public readonly int val;

	public AllReceiveMessageEvent (string id, string message1, string message2, int val=-1) {
		this.id = id;
		this.message1 = message1;
		this.message2 = message2;
		this.val = val;
	}
}
