using UnityEngine;
using System.Collections;

public class FoundGamesEvent : GameEvent {

	public HostData[] hosts;

	public FoundGamesEvent (HostData[] hosts) {
		this.hosts = hosts;
	}
}
