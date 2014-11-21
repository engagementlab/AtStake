using UnityEngine;
using System.Collections;

public class ChangeStateEvent : GameEvent {

	public GameState state;

	public ChangeStateEvent (GameState state) {
		this.state = state;
	}
}
