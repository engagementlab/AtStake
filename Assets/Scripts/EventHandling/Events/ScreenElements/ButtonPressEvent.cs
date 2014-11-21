using UnityEngine;
using System.Collections;

public class ButtonPressEvent : GameEvent {

	public string id = "";

	public ButtonPressEvent (string id) {
		this.id = id;
	}
}
