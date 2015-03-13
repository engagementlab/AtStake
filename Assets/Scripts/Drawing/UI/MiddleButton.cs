using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MiddleButton : MonoBehaviour {

	public Image source;
	public Button button;
	public Text text;

	public ButtonElement Element { get; private set; }
	public GameScreen Screen { 
		get { return Element.screen; }
	}
	public string ID { 
		get { return Element.id; }
	}
	public string Content { 
		get { 
			return Element.Content; 
		}
	}

	public virtual void Set (ButtonElement element) {
		SetEnabled (true);
		Element = element;
		text.text = Content;
		SetColor (element.Color);
		OnSet ();
	}

	public virtual void OnSet () {
		//transform.SetLocalScale (1);
		//Invoke ("Expand", SlideController.SlideTime);
 	}

	public void SetColor (string color) {
		ButtonColor buttonColor = ButtonManager.instance.GetColor (color);
		source.sprite = buttonColor.source;
		SpriteState ss = button.spriteState;
		ss.pressedSprite = buttonColor.pressed;
		button.spriteState = ss;
	}

	public void SetEnabled (bool enabled) {
		source.enabled = enabled;
		text.enabled = enabled;
	}

	void Expand () {
		StartCoroutine (CoExpand ());		
	}

	IEnumerator CoExpand () {
		
		float time = 0.5f;
		float eTime = 0f;
		float bulge = 0.05f;
	
		while (eTime < time/2) {
			eTime += Time.deltaTime;
			float progress = Mathf.SmoothStep (0, 1, eTime / time/2);
			transform.SetLocalScale (1 + progress * bulge);
			yield return null;
		}

		while (eTime < time) {
			eTime += Time.deltaTime;
			float progress = Mathf.SmoothStep (1, 0, eTime / time);
			transform.SetLocalScale (1 + progress * bulge);
			yield return null;
		}
	}
}
