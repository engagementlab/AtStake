using UnityEngine;
using System.Collections;

public class TimerElement : ScreenElement {

	Timer timer = null;

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
}
