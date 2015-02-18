using UnityEngine;
using System.Collections;

public class ChangeScreenEvent : GameEvent {

	public readonly GameScreen screen;
	public readonly bool back = false;

	public ChangeScreenEvent (GameScreen screen, bool back) {
		this.screen = screen;
		this.back = back;
	}
}
