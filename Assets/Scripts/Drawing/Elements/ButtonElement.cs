using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ButtonElement : ScreenElement {

	public readonly string id = "";
	public readonly GameScreen screen;
	string content = "";
	public string Content {
		get { return content; }
		set { 
			content = value; 
			if (text != null) 
				text.text = content;
		}
	}
	protected Text text = null;

	public ButtonElement (GameScreen screen, string id, string content, int position) {
		this.screen = screen;
		this.id = id;
		this.content = content;
		this.Position = position;
	}

	public void SetText (Text text) {
		this.text = text;
		this.text.text = content;
		//StyleText ();
	}

	void StyleText () {
		/*text.fontSize = style.Size;
		text.alignment = style.Anchor;
		text.color = style.Color;*/
	}
}
