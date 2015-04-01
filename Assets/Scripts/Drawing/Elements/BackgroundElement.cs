using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class BackgroundElement : ScreenElement {

	public string BackgroundName { get; private set; }
	public Sprite Sprite {
		get { return ImageManager.instance.GetBackground (BackgroundName); }
	}

	Color color;
	public Color Color { 
		get { return color; }
		private set {
			if (value == Color.black) {
				value.a = 0.05f;
			}
			if (value == Color.white) {
				value.a = 0.2f;
			}
			color = value;
		}
	}

	string anchor = "bottom";
	public string Anchor {
		get { return anchor; }
	}

	public BackgroundElement (string backgroundName, Color color, string anchor="bottom") {
		BackgroundName = backgroundName;
		Color = color;
		this.anchor = anchor;
	}
}
