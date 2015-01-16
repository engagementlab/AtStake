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
	Text text = null;


	public LabelElement (string content="") {
		this.content = content;
	}

	public void SetText (Text text) {
		this.text = text;
		this.text.text = content;
	}
}
