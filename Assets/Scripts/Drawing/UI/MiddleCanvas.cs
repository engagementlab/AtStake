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
		Events.instance.AddListener<UpdateDrawerEvent> (OnUpdateDrawerEvent);
	}
 
	public void OnButtonPress (MiddleButton button) {
		Events.instance.Raise (new ButtonPressEvent (button.Screen, button.ID));
	}

	public void OnEndEditTextField (InputField inputField) {
		textFieldManager.OnEndEdit (inputField.text);
	}

	void UpdateScreen () {
		labelManager.RemoveLabels ();
		textFieldManager.Hide ();
		buttonManager.RemoveButtons ();

		foreach (ScreenElement element in elements) {
			if (element is LabelElement) {
				LabelElement l = element as LabelElement;
				labelManager.SetLabel (l);
			}
			if (element is TextFieldElement) {
				TextFieldElement t = element as TextFieldElement;
				textFieldManager.Show (t);
			}
			if (element is ButtonElement && !(element is BottomButtonElement)) {
				ButtonElement b = element as ButtonElement;
				buttonManager.SetButton (b.screen, b.id, b.Content);
			}
		}
	}

	void SortElements () {
		List<GameObject> gameObjects;
		gameObjects.AddRange (buttonManager.Buttons);
		gameObjects.AddRange (labelManager.Labels);
		gameObjects.Add (textFieldManager.inputField);
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

[System.Serializable]
public class ElementManager : System.Object {
	public RectTransform buttonGroupTransform;
}

[System.Serializable]
public class MiddleButtonManager : ElementManager {

	public GameObject button;
	List<GameObject> buttons = new List<GameObject> ();
	public List<GameObject> Buttons {
		get { return buttons; }
	}

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

	public GameObject label;
	List<GameObject> labels = new List<GameObject> ();
	public List<GameObject> Labels {
		get { return labels; }
	}

	public void SetLabel (LabelElement label) {

		GameObject go = GetInactiveLabel ();
		if (go == null) {
			go = CreateLabel ();
			go.SetActive (true);
		}

		Text t = go.GetComponent<Text> ();
		label.SetText (t);
	}

	GameObject CreateLabel () {
		GameObject go = GameObject.Instantiate (label) as GameObject;
		RectTransform t = go.transform as RectTransform;
		t.SetParent (buttonGroupTransform);
		t.SetSiblingIndex (0);
		t.localScale = ExtensionMethods.Vector3One;
		labels.Add (go);
		return go;
	}

	GameObject GetInactiveLabel () {
		foreach (GameObject go in labels) {
			if (!go.activeSelf) {
				go.SetActive (true);
				return go;
			}
		}
		return null;
	}

	public void RemoveLabels () {
		foreach (GameObject go in labels) {
			go.SetActive (false);
		}
	}
}

[System.Serializable]
public class MiddleTextFieldManager : ElementManager {

	public TextFieldElement textField;
	public InputField inputField;
	public Text placeholder;
	public Text text;

	public void SetPlaceholder (string content) {
		placeholder.text = content;
	}

	public void Show (TextFieldElement textField) {
		this.textField = textField;
		inputField.gameObject.SetActive (true);
		text.text = textField.content;
	}

	public void OnEndEdit (string content) {
		textField.content = content;
	}

	public void Hide () {
		inputField.gameObject.SetActive (false);
	}
}