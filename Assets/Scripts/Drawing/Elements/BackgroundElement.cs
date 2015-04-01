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
				value.a = 0.25f;
			}
			if (value == Color.white) {
				value.a = 0.5f;
			}
			color = value;
		}
	}

	public BackgroundElement (string backgroundName, Color color) {
		BackgroundName = backgroundName;
		Color = color;
	}
}
