using UnityEngine;
using System.Collections;

public class ClientConfirmMessageEvent : GameEvent {

	public readonly string message;

	public ClientConfirmMessageEvent (string message) {
		this.message = message;
	}
}
