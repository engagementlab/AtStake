using UnityEngine;
using System.Collections;

public class RefreshPlayerListEvent : GameEvent {

	public string[] playerNames;

	public RefreshPlayerListEvent (string[] playerNames) {
		this.playerNames = playerNames;
	}
}
