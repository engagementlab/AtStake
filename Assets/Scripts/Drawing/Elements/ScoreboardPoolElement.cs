using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ScoreboardPoolElement : ScreenElement {

	string content = "";
	public string Content {
		get { return content; }
		set { 
			content = value; 
			if (text != null) {
				text.text = content;
			}
		}
	}

	Text text = null;

	public ScoreboardPoolElement (string name, int score, int position) {
		SetContent (name, score);
		this.Position = position;
	}

	public void SetContent (string name, int score) {
		Content = string.Format ("{0}: {1}", name, score);
	}

	public void SetText (Text text) {
		this.text = text;
		text.color = Palette.Grey;
		text.text = content;
	}
}
