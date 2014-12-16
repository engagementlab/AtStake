using UnityEngine;
using System.Collections;

public class PlayerSendMessageEvent : GameEvent {

	public readonly string message;

	public PlayerSendMessageEvent (string message) {
		this.message = message;
	}
}
