using UnityEngine;
using System.Collections;

public delegate void OnUpdateContent (string content);

public class TextFieldElement : ScreenElement {

	public OnUpdateContent onUpdateContent;
	
	string content = "";
	public string Content {
		get { return content; }
		set { 
			content = value;
			if (onUpdateContent != null) onUpdateContent (content);
		}
	}

	public TextFieldElement (int position) {
		this.Position = position;
	}
}
