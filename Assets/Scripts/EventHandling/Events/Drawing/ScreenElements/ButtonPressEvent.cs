using UnityEngine;
using System.Collections;

public class ButtonPressEvent : GameEvent {

	public readonly string id = "";
	public readonly GameScreen screen;

	public ButtonPressEvent (GameScreen screen, string id) {
		this.screen = screen;
		this.id = id;
	}
}
