using UnityEngine;
using System.Collections;

public class OthersSendMessageEvent : GameEvent {

	public readonly string playerName;
	public readonly string message;
	
	public OthersSendMessageEvent (string playerName, string message="") {
		this.playerName = playerName;
		this.message = message;
	}
}
