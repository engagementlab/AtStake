using UnityEngine;
using System.Collections;

public class OthersReceiveMessageEvent : GameEvent {

	public readonly string playerName;
	public readonly string message;
	
	public OthersReceiveMessageEvent (string playerName, string message="") {
		this.playerName = playerName;
		this.message = message;
	}
}
