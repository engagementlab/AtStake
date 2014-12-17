using UnityEngine;
using System.Collections;

public class PlayersReceiveMessageEvent : GameEvent {

	public readonly string message1;
	public readonly string message2;

	public PlayersReceiveMessageEvent (string message1, string message2) {
		this.message1 = message1;
		this.message2 = message2;
	}
}
