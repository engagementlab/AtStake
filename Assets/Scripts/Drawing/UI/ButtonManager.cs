using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ButtonManager : MonoBehaviour {

	public ButtonColor timer;
	public ButtonColor blue;
	public ButtonColor green;
	public ButtonColor orange;
	public ButtonColor pink;
	public ButtonColor bottomOrange;
	public ButtonColor bottomPink;

	public static ButtonManager instance = null;

	void Awake () {
		if (instance == null)
			instance = this;
	}

	public ButtonColor GetColor (string color) {
		switch (color) {
			case "timer": return timer;
			case "blue": return blue;
			case "green": return green;
			case "orange": return orange;
			case "bottomOrange": return bottomOrange;
			case "bottomPink": return bottomPink;
		}
		return new ButtonColor ();
	}
}

[System.Serializable]
public class ButtonColor : System.Object {
	public Sprite source = null;
	public Sprite pressed = null;
}