using UnityEngine;
using System.Collections;

public class ButtonPressEvent : GameEvent {

	public readonly ButtonElement element;
	public readonly string id = "";
	public readonly GameScreen screen;

	public ButtonPressEvent (ButtonElement element) {
		this.element = element;
		screen = element.screen;
		id = element.id;
	}
}
