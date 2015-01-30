using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TimerButton : MiddleButton {

	public Image backImage;
	public Image frontImage;

	public override void Set (ButtonElement element) {
		base.Set (element);
		SetColor ("timer");
		
		Color back = Color.grey;
		back.a = 0.25f;
		backImage.color = back;
		frontImage.color = Color.grey;
	}
}
