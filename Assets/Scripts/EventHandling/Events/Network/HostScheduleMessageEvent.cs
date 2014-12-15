using UnityEngine;
using System.Collections;

public class HostScheduleMessageEvent : GameEvent {
	
	public readonly string message;

	public HostScheduleMessageEvent (string message) {
		this.message = message;
	}
}
