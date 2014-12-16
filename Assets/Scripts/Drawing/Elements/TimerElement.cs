using UnityEngine;
using System.Collections;

public class TimerElement : ScreenElement {

	GameScreen screen;
	Timer timer;

	float startTime = 0f;
	public float Seconds {
		get { 
			if (timer.Seconds == -1f) {
				return startTime;
			} else {
				return timer.Seconds; 
			}
		}
	}

	public int SecondsRounded {
		get { return Mathf.CeilToInt (Seconds); }
	}

	public TimerElement (GameScreen screen, float startTime) {
		this.screen = screen;
		this.startTime = startTime;
		timer = Timer.instance;
	}

	public void StartCountDown () {
		timer.StartCountDown (this, startTime);
	}

	public void OnCountDownEnd () {
		screen.OnCountDownEnd ();
	}
}
