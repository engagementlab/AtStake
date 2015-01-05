using UnityEngine;
using System.Collections;

public class MessagesMatchEvent : GameEvent {

	public readonly string id;
	public readonly string message;

	public MessagesMatchEvent (string id, string message) {
		this.id = id;
		this.message = message;
	}
}
