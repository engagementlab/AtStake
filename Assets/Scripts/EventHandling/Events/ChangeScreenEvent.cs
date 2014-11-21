using UnityEngine;
using System.Collections;

public class ChangeScreenEvent : GameEvent {

	public GameScreen screen;

	public ChangeScreenEvent (GameScreen screen) {
		this.screen = screen;
	}
}
