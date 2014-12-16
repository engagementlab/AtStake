using UnityEngine;
using System.Collections;

public class SendMessageToPlayerEvent : GameEvent {

	public readonly string playerName;
	public readonly string message;

	public SendMessageToPlayerEvent (string playerName, string message="") {
		this.playerName = playerName;
		this.message = message;
	}
}
