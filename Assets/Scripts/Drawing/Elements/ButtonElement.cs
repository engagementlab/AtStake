	using UnityEngine;
using System.Collections;

public class ButtonElement : ScreenElement {

	public readonly string id = "";
	public readonly GameScreen screen;
	string content = "";
	public string Content {
		get { return content; }
		set { content = value; }
	}

	public ButtonElement (GameScreen screen, string id, string content, int position) {
		this.screen = screen;
		this.id = id;
		this.content = content;
		this.Position = position;
	}
}
