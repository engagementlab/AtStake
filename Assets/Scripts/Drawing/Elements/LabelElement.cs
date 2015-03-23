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
		/*text.color = new Color (text.color.r, text.color.g, text.color.b, 0f);
		CoroutineManager.Instance.WaitForSeconds (SlideController.SlideTime, FadeText);*/
	}

	void StyleText () {
		text.fontSize = style.Size;
		text.alignment = style.Anchor;
		text.color = style.Color;
		text.fontStyle = style.FontStyle;
	}

	void FadeText () {
		CoroutineManager.Instance.FloatLerp (0, 1, 0.5f, OnFloatLerp);
	}

	void OnFloatLerp (float progress) {
		Color color = text.color;
		text.color = new Color (color.r, color.g, color.b, progress);
	}
}
