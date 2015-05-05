using UnityEngine;
using System.Collections;

public class ForceDisconnectEvent : GameEvent {

	public readonly bool wasHost = false;
	
	public ForceDisconnectEvent (bool wasHost) {
		this.wasHost = wasHost;
	} 
}
