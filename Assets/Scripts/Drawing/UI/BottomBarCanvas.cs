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

	void SetButton (GameScreen gameScreen, string id, string content, Side side) {
		if (side == Side.Left) {
			buttonLeft.Set (gameScreen, id, content, Palette.Pink);
		} else {
			buttonRight.Set (gameScreen, id, content, Palette.Orange);
		}
	}

	void DisableButtons () {
		buttonLeft.SetEnabled (false);
		buttonRight.SetEnabled (false);
	}

	public void OnButtonPress (BottomButton button) {
		Events.instance.Raise (new ButtonPressEvent (button.Screen, button.ID));
	}

	void UpdateScreen () {
		DisableButtons ();
		foreach (ScreenElement element in elements) {
			if (element is BottomButtonElement) {
				BottomButtonElement b = element as BottomButtonElement;
				SetButton (b.screen, b.id, b.Content, b.Side);
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
