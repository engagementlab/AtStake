using UnityEngine;
using System.Collections;

public class HostSendMessageEvent : GameEvent {

	public readonly string name, message1, message2;
	public readonly int val;

	public HostSendMessageEvent (string name, string message1="", string message2="", int val=-1) {
		this.name = name;
		this.message1 = message1;
		this.message2 = message2;
		this.val = val;
	}
}
