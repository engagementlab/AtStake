using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MiddleCanvas : MonoBehaviour {

	public Sprite buttonSprite;
	float buttonAspectRatio;
	float ButtonHeight {
		get { return buttonGroupWidth * buttonAspectRatio + buttonGroupSpacing; }
	}

	public RectTransform buttonGroupTransform;
	public VerticalLayoutGroup buttonGroup;
	float buttonGroupWidth;
	float buttonGroupSpacing;

	public GameObject button;

	void Awake () {
		buttonAspectRatio = buttonSprite.rect.height / buttonSprite.rect.width;
		buttonGroupWidth = buttonGroupTransform.rect.width;
		buttonGroupSpacing = buttonGroup.spacing;
		CreateButton ();
		SetButtonGroupHeight (5);
	}

	void CreateButton () {
		GameObject go = Instantiate (button) as GameObject;
		RectTransform t = go.transform as RectTransform;
		t.SetParent (buttonGroupTransform);
	}

	void SetButtonGroupHeight (int buttonCount) {
		buttonGroupTransform.SetHeight (ButtonHeight * buttonCount);
	}
}
