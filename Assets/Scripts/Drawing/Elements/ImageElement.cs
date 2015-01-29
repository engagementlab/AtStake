using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ImageElement : ScreenElement {

	public string SpriteName { get; private set; }
	public Sprite Sprite {
		get { return ImageManager.instance.GetSprite (SpriteName); }
	}

	public Color Color { get; private set; }

	public ImageElement (string spriteName, int position, Color color) {
		SpriteName = spriteName;
		Position = position;
		Color = color;
	}
}
