using UnityEngine;
using System.Collections;

public class ButtonElement : ScreenElement {

	public readonly string id = "";
	public readonly string content = "";

	public ButtonElement (string id, string content) {
		this.id = id;
		this.content = content;
	}

	public override void Draw () {
		if (GUILayout.Button (content)) {
			Events.instance.Raise (new ButtonPressEvent (id));
		}
	}
}
