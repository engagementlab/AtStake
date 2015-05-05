using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TimerElement : ButtonElement {

	Timer timer = null;
	Button button = null;
	Image fill = null;
	Text text = null;

	public override string Content {
		get { return content.ToLower (); }
		set { 
			content = value.ToLower (); 
			if (text != null) 
				text.text = content;
		}
	}

	new bool interactable = true;
	public bool Interactable {
		get { return interactable; }
		set { 
			interactable = value; 
			if (button != null)	
				button.interactable = interactable;
		}
	}

	public float Seconds {
		get { 
			if (timer == null) {
				timer = Timer.instance;
			}
			if (timer.Seconds == -1f) {
				return 0;
			} else {
				return timer.Seconds; 
			}
		}
	}

	public int SecondsRounded {
		get { return Mathf.CeilToInt (Seconds); }
	}

	public TimerElement (GameScreen screen, string id, string content, int position) : base (screen, id, content, position, "white") {
		timer = Timer.instance;
	}

	public void SetButton (Button button, Image fill, Text text) {
		this.button = button;
		this.fill = fill;
		this.text = text;
		button.interactable = Interactable;
	}

	public void Update () {
		if (interactable && timer.Seconds == 0 || timer.Seconds == -1)
			fill.fillAmount = 1;
		else
			fill.fillAmount = timer.Progress;
	}
}
