using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class BottomBarCanvas : MonoBehaviour {

	public Image buttonLeft;
	public Image buttonRight;
	public Text leftText;
	public Text rightText;

	void Awake () {
		SetLeft (Palette.Blue, "back");
		SetRight (Palette.Teal, "next");
	}

	public void SetLeft (Color color, string content) {
		buttonLeft.color = color;
		leftText.text = content;
	}

	public void SetRight (Color color, string content) {
		buttonRight.color = color;
		rightText.text = content;
	}
}
