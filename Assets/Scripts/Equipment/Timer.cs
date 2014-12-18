using UnityEngine;
using System.Collections;

[RequireComponent (typeof (NetworkView))]
public class Timer : MonoBehaviour {

	float seconds = -1f;
	public float Seconds {
		get { return seconds; }
	}

	bool countingDown = false;

	TimerElement timerElement;
	static public Timer instance;

	void Awake () {
		if (instance == null)
			instance = this;
	}

	public void SetTime (float seconds) {
		if (!countingDown) {
			this.seconds = seconds;
		}
	}

	public void StartCountDown (TimerElement timerElement, float duration) {
		if (countingDown)
			return;
		this.timerElement = timerElement;
		seconds = duration;
		StartCoroutine (CountDown ());
	}

	public void AddSeconds (float amount) {
		if (!countingDown) {
			seconds = amount;
			StartCoroutine (CountDown ());
		} /*else {
			seconds += amount;
		}*/
	}

	IEnumerator CountDown () {
		countingDown = true;
		while (seconds > 0f) {
			seconds -= Time.deltaTime;
			yield return null;
		}
		countingDown = false;
		OnCountDownEnd ();
	}

	void OnCountDownEnd () {
		timerElement.OnCountDownEnd ();
	}
}
