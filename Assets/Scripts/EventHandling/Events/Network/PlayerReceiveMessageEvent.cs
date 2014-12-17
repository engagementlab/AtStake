using UnityEngine;
using System.Collections;

public class PlayerReceiveMessageEvent : GameEvent {

	public readonly string message;

	public PlayerReceiveMessageEvent (string message) {
		this.message = message;
	}
}
