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
		} else {
			buttonRight.Set (element);
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
			}
		}
	}

	void OnChangeScreenEvent (ChangeScreenEvent e) {
		screen = e.screen;
		elements = screen.Elements;
		UpdateScreen ();		
	}

	void OnUpdateDrawerEvent (UpdateDrawerEvent e) {
		elements = screen.Elements;
		UpdateScreen ();
	}
}
