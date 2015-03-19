using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;

public class MiddleCanvas : MonoBehaviour {

	public Scrollbar scrollbar;
	public ScrollRect scrollRect;
	public VerticalLayoutGroup buttonGroup;
	public RectTransform buttonGroupTransform;
	public MiddleButtonManager buttonManager;
	public MiddleLabelManager labelManager;
	public MiddleTextFieldManager textFieldManager;
	public MiddleTimerManager timerManager;
	public MiddleImageManager imageManager;
	public ScoreboardPoolManager scoreboardPoolManager;
	public ScoreboardPotManager scoreboardPotManager;

	ScreenElement[] elements;
	GameScreen screen;

	void Awake () {
		AlignMiddle ();
	}

	void Start () {
		ResetScrollbar ();
		SetScrollbarActive (false);
	}

	public void AlignMiddle () {
		buttonGroup.padding.top = 0; // 100
		buttonGroup.childAlignment = TextAnchor.MiddleCenter;
		buttonGroupTransform.pivot = new Vector2 (0.5f, 0.5f);
		buttonGroupTransform.anchorMin = new Vector2 (0.5f, 0.5f);
		buttonGroupTransform.anchorMax = new Vector2 (0.5f, 0.5f); 
		buttonGroupTransform.anchoredPosition = new Vector2 (0, -38);
		buttonGroupTransform.sizeDelta = new Vector2 (750, 1009);
	}

	public void AlignUpper () {
		buttonGroup.padding.top = 0; // 120
		buttonGroup.childAlignment = TextAnchor.UpperCenter;
		buttonGroupTransform.pivot = new Vector2 (0.5f, 1);
		buttonGroupTransform.anchorMin = new Vector2 (0.5f, 1);
		buttonGroupTransform.anchorMax = new Vector2 (0.5f, 1); 
		buttonGroupTransform.anchoredPosition = new Vector2 (0, -200);
		buttonGroupTransform.sizeDelta = new Vector2 (750, 1009);
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
		
		SetScrollbarActive (false);

		labelManager.RemoveLabels ();
		textFieldManager.Hide ();
		timerManager.Hide ();
		imageManager.Hide ();
		scoreboardPoolManager.Hide ();
		scoreboardPotManager.Hide ();
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
			if (element is ScoreboardPoolElement) {
				ScoreboardPoolElement s = element as ScoreboardPoolElement;
				scoreboardPoolManager.Show (s);
			}
			if (element is ScoreboardPotElement) {
				ScoreboardPotElement s = element as ScoreboardPotElement;
				scoreboardPotManager.Show (s);
			}
		}

		CoroutineManager.Instance.WaitForFrame (OnWaitForFrame);
	}

	void OnWaitForFrame () {
		if (buttonGroupTransform.sizeDelta.y > 750) {
			SetScrollbarActive (true);
			ResetScrollbar ();
		}
	}

	void UpdateAlignment (TextAnchor alignment) {
		if (alignment == TextAnchor.MiddleCenter) {
			AlignMiddle ();
		}
		if (alignment == TextAnchor.UpperCenter) {
			AlignUpper ();
		}
	}

	public void OnChangeScreenEvent (ChangeScreenEvent e) {
		screen = e.screen;
		elements = screen.Elements;
		UpdateAlignment (screen.Alignment);
		UpdateScreen ();
	}

	public void OnUpdateDrawerEvent (UpdateDrawerEvent e) {
		if (elements != null) {
			elements = screen.Elements;
			UpdateScreen ();
		}
	}

	void ResetScrollbar () {
		scrollbar.value = 1;
	}

	void SetScrollbarActive (bool active) {
		if (active) {
			buttonGroup.padding.top = 100; 
		} else {
			buttonGroup.padding.top = 0;
		}
		scrollRect.enabled = active;
		scrollbar.gameObject.SetActive (active);
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

		if (t.text == "") {
			go.SetActive (false);
		}
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
	public TimerButton timerButton;
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
		timerButton.Set (timerElement);
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

[System.Serializable]
public class ScoreboardPoolManager : ElementManager {

	public GameObject scoreboardPoolContainer;
	List<GameObject> poolContainers = new List<GameObject> ();
	public List<GameObject> PoolContainers {
		get { return poolContainers; }
	}

	public void Show (ScoreboardPoolElement scoreboardPoolElement) {
		
		GameObject go = GetInactivePool ();
		if (go == null) {
			go = CreatePool ();
			go.SetActive (true);
		}
		go.transform.SetSiblingIndex (scoreboardPoolElement.Position);
		Text t = go.GetComponent<ScoreboardPoolContainer> ().text;
		scoreboardPoolElement.SetText (t);
	}

	GameObject CreatePool () {
		GameObject go = GameObject.Instantiate (scoreboardPoolContainer) as GameObject;
		RectTransform t = go.transform as RectTransform;
		t.SetParent (buttonGroupTransform);
		t.localScale = ExtensionMethods.Vector3One;
		poolContainers.Add (go);
		return go;
	}

	GameObject GetInactivePool () {
		foreach (GameObject go in poolContainers) {
			if (!go.activeSelf) {
				go.SetActive (true);
				return go;
			}
		}
		return null;
	}

	public void Hide () {
		foreach (GameObject go in poolContainers) {
			go.SetActive (false);
		}
	}
}

[System.Serializable]
public class ScoreboardPotManager : ElementManager {

	public GameObject scoreboardPotContainer;
	public Text text;

	public void Show (ScoreboardPotElement scoreboardPotElement) {
		scoreboardPotContainer.SetActive (true);
		scoreboardPotContainer.transform.SetSiblingIndex (scoreboardPotElement.Position);
		text.text = scoreboardPotElement.Content;
	}

	public void Hide () {
		scoreboardPotContainer.SetActive (false);
	}
}