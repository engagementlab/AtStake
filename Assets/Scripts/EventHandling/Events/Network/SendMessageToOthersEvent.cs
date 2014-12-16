using UnityEngine;
using System.Collections;

public class SendMessageToOthersEvent : GameEvent {

	public readonly string playerName;
	public readonly string message;

	public SendMessageToOthersEvent (string playerName, string message="") {
		this.playerName = playerName;
		this.message = message;
	}
}
