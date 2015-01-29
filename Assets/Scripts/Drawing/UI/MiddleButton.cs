using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MiddleButton : MonoBehaviour {

	public Image source;
	public Button button;
	public Text text;

	public ButtonElement Element { get; private set; }
	public GameScreen Screen { 
		get { return Element.screen; }
	}
	public string ID { 
		get { return Element.id; }
	}
	public string Content { 
		get { 
			return Element.Content; 
		}
	}

	public void Set (ButtonElement element) {
		SetEnabled (true);
		Element = element;
		text.text = Content;
		SetColor (element.Color);
	}

	public void SetColor (string color) {
		ButtonColor buttonColor = ButtonManager.instance.GetColor (color);
		source.sprite = buttonColor.source;
		SpriteState ss = button.spriteState;
		ss.pressedSprite = buttonColor.pressed;
		button.spriteState = ss;
	}

	public void SetEnabled (bool enabled) {
		source.enabled = enabled;
		text.enabled = enabled;
	}
}
