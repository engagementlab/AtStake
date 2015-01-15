using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;

public class MiddleCanvas : MonoBehaviour {

	public MiddleButtonManager buttonManager;
	public MiddleLabelManager labelManager;
	public MiddleTextFieldManager textFieldManager;

	ScreenElement[] elements;
	GameScreen screen;

	void Awake () {
		Events.instance.AddListener<ChangeScreenEvent> (OnChangeScreenEvent);
	}

	public void OnButtonPress (MiddleButton button) {
		Events.instance.Raise (new ButtonPressEvent (button.Screen, button.ID));
	}

	void OnChangeScreenEvent (ChangeScreenEvent e) {
		
		screen = e.screen;
		elements = screen.Elements;

		labelManager.ResetLabel ();
		textFieldManager.Hide ();
		buttonManager.RemoveButtons ();

		foreach (ScreenElement element in elements) {
			if (element is LabelElement) {
				LabelElement l = element as LabelElement;
				labelManager.SetLabel (l.content);
			}
			if (element is TextFieldElement) {
				TextFieldElement t = element as TextFieldElement;
				textFieldManager.Show (t.content);
			}
			if (element is ButtonElement) {
				ButtonElement b = element as ButtonElement;
				buttonManager.SetButton (b.screen, b.id, b.Content);
			}
		}
	}
}

[System.Serializable]
public class ElementManager : System.Object {
	public RectTransform buttonGroupTransform;
}

[System.Serializable]
public class MiddleButtonManager : ElementManager {

	public GameObject button;
	List<GameObject> buttons = new List<GameObject> ();

	public void SetButton (GameScreen gameScreen, string id, string content) {
		
		GameObject go = GetInactiveButton ();
		if (go == null) {
			go = CreateButton ();
		}

		MiddleButton mb = go.GetComponent<MiddleButton> ();
		mb.Set (gameScreen, id, content);
	}

	GameObject CreateButton () {
		GameObject go = GameObject.Instantiate (button) as GameObject;
		RectTransform t = go.transform as RectTransform;
		t.SetParent (buttonGroupTransform);
		t.localScale = ExtensionMethods.Vector3One;
		buttons.Add (go);
		return go;
	}

	GameObject GetInactiveButton () {
		foreach (GameObject go in buttons) {
			if (!go.activeSelf) {
				go.SetActive (true);
				return go;
			}
		}
		return null;
	}

	public void RemoveButtons () {
		foreach (GameObject go in buttons) {
			go.SetActive (false);
		}
	}
}

[System.Serializable]
public class MiddleLabelManager : ElementManager {

	public Text text;

	public void SetLabel (string content) {
		text.text = content;
		if (content == "") {
			text.gameObject.SetActive (false);
		} else {
			text.gameObject.SetActive (true);
		}
	}

	public void ResetLabel () {
		SetLabel ("");
	}
}

[System.Serializable]
public class MiddleTextFieldManager : ElementManager {

	public InputField inputField;
	public Text placeholder;
	public Text text;

	public void SetPlaceholder (string content) {
		placeholder.text = content;
	}

	public void Show (string content) {
		inputField.gameObject.SetActive (true);
		text.text = content;
		Debug.Log(content);
	}

	public void Hide () {
		inputField.gameObject.SetActive (false);
	}
}