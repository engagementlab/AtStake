using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TimerButton : MiddleButton {

	public Image backImage;
	public Image frontImage;

	public override void Set (ButtonElement element) {
		base.Set (element);
		SetColor ("timer");
	}
}
