using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class LabelElement : ScreenElement {

	string content = "";
	public string Content {
		get { return content; }
		set { 
			content = value;
			if (text != null)
				text.text = content;
		}
	}
	TextStyle style = new DefaultTextStyle ();
	Text text = null;

	public LabelElement (string content, int position, TextStyle style=null) {
		this.content = content;
		if (style == null) {
			style = new DefaultTextStyle ();
		}
		this.style = style;
		this.Position = position;
	}

	public void SetText (Text text) {
		this.text = text;
		this.text.text = content;
		StyleText ();
	}

	void StyleText () {
		text.fontSize = style.Size;
		text.alignment = style.Anchor;
		text.color = style.Color;
		text.fontStyle = style.FontStyle;
	}
}
