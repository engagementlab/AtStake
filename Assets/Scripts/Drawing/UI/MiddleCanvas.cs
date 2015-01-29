using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;

public class MiddleCanvas : MonoBehaviour {

	public MiddleButtonManager buttonManager;
	public MiddleLabelManager labelManager;
	public MiddleTextFieldManager textFieldManager;
	public MiddleTimerManager timerManager;
	public MiddleImageManager imageManager;

	ScreenElement[] elements;
	GameScreen screen;

	void Awake () {
		Events.instance.AddListener<ChangeScreenEvent> (OnChangeScreenEvent);
		Events.instance.AddListener<UpdateDrawerEvent> (OnUpdateDrawerEvent);
	}
 
	public void OnButtonPress (MiddleButton button) {
		Events.instance.Raise (new ButtonPressEvent (button.Element));
	}

	public void OnEndEditTextField (InputField inputField) {
		textFieldManager.OnEndEdit (inputField.text);
	}

	public void OnTimerPress (TimerButton timer) {
		Events.instance.Raise (new ButtonPressEvent (timer.Element));
	}

	void Update () {
		if (timerManager.Enabled) {
			timerManager.UpdateProgress ();
		}
	}

	void UpdateScreen () {
		
		labelManager.RemoveLabels ();
		textFieldManager.Hide ();
		timerManager.Hide ();
		imageManager.Hide ();
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
			if (element is ButtonElement && !(element is BottomButtonElement) && !(element is TimerElement)) {
				ButtonElement b = element as ButtonElement;
				buttonManager.SetButton (b);
			}
			if (element is TimerElement) {
				TimerElement t = element as TimerElement;
				timerManager.Show (t);
			}
			if (element is ImageElement) {
				ImageElement i = element as ImageElement;
				imageManager.Show (i);
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

	public void SetButton (ButtonElement button) {

		GameObject go = GetInactiveButton ();
		if (go == null) {
			go = CreateButton ();
			go.SetActive (true);
		}
		go.transform.SetSiblingIndex (button.Position);
		MiddleButton mb = go.GetComponent<MiddleButton> ();
		mb.Set (button);
		button.MiddleButton = mb;
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
		go.transform.SetSiblingIndex (label.Position);
		Text t = go.GetComponent<Text> ();
		label.SetText (t);
	}

	GameObject CreateLabel () {
		GameObject go = GameObject.Instantiate (label) as GameObject;
		RectTransform t = go.transform as RectTransform;
		t.SetParent (buttonGroupTransform);
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
		inputField.transform.SetSiblingIndex (textField.Position);
		text.text = textField.content;
	}

	public void OnEndEdit (string content) {
		textField.content = content;
	}

	public void Hide () {
		inputField.gameObject.SetActive (false);
	}
}

[System.Serializable]
public class MiddleTimerManager : ElementManager {

	public TimerElement timerElement;
	public GameObject timer;
	public Image fill;
	public Button button;
	public Text text;
	public bool Enabled { get; set; }

	public MiddleTimerManager () {
		Enabled = false;
	}

	public void Show (TimerElement timerElement) {
		this.timerElement = timerElement;
		timerElement.SetButton (button, fill, text);
		timer.SetActive (true);
		timer.transform.SetSiblingIndex (timerElement.Position);
		TimerButton tb = timer.GetComponent<TimerButton> ();
		tb.Set (timerElement);
		Enabled = true;
	}

	public void Hide () {
		timer.SetActive (false);
		Enabled = false;
	}

	public void UpdateProgress () {
		if (timerElement != null)
			timerElement.Update ();
	}
}

[System.Serializable]
public class MiddleImageManager : ElementManager {

	public GameObject image;

	public void Show (ImageElement imageElement) {
		image.SetActive (true);
		image.transform.SetSiblingIndex (imageElement.Position);
		StaticImage staticImage = image.GetComponent<StaticImage> ();
		staticImage.Source = imageElement.Sprite;
		staticImage.Color = imageElement.Color;
	}

	public void Hide () {
		image.SetActive (false);
	}
}