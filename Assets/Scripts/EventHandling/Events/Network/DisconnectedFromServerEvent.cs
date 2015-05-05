using UnityEngine;
using System.Collections;

public class DisconnectedFromServerEvent : GameEvent {
	
	public readonly bool wasHost = false;
	
	public DisconnectedFromServerEvent (bool wasHost) {
		this.wasHost = wasHost;
	}
}
