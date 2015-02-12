using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class BottomBarCanvas : MonoBehaviour {

	public BottomButton buttonLeft;
	public BottomButton buttonRight;

	GameScreen screen;
	ScreenElement[] elements;

	void Awake () {
		Events.instance.AddListener<ChangeScreenEvent> (OnChangeScreenEvent);
		Events.instance.AddListener<UpdateDrawerEvent> (OnUpdateDrawerEvent);
	}

	void SetButton (Side side, ButtonElement element) {
		if (side == Side.Left) {
			buttonLeft.Set (element);
			buttonLeft.SetEnabled (true);
		} else {
			buttonRight.Set (element);
			buttonRight.SetEnabled (true);
		}
	}

	void DisableButtons () {
		buttonLeft.SetEnabled (false);
		buttonRight.SetEnabled (false);
	}

	public void OnButtonPress (BottomButton button) {
		Events.instance.Raise (new ButtonPressEvent (button.Element));
	}

	void UpdateScreen () {
		DisableButtons ();
		foreach (ScreenElement element in elements) {
			if (element is BottomButtonElement) {
				BottomButtonElement b = element as BottomButtonElement;
				SetButton (b.Side, b);
				if (b.Side == Side.Left) {
					b.MiddleButton = buttonLeft;
				} else {
					b.MiddleButton = buttonRight;
				}
			}
		}
	}

	void OnChangeScreenEvent (ChangeScreenEvent e) {
		screen = e.screen;
		elements = screen.Elements;
		UpdateScreen ();		
	}

	void OnUpdateDrawerEvent (UpdateDrawerEvent e) {
		if (elements != null) {
			elements = screen.Elements;
			UpdateScreen ();
		}
	}
}
