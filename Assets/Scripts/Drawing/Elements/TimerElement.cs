using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TimerElement : ButtonElement {

	Timer timer = null;
	Image fill = null;

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

	public TimerElement (GameScreen screen, string id, string content, int position) : base (screen, id, content, position) {
		timer = Timer.instance;
	}

	public void SetFill (Image fill) {
		this.fill = fill;
	}

	public void Update () {
		fill.fillAmount = timer.Progress;
	}
}
