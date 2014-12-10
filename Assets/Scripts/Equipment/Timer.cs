using UnityEngine;
using System.Collections;

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

	public void StartCountDown (TimerElement timerElement, float duration) {
		this.timerElement = timerElement;
		seconds = duration;
		StartCoroutine (CountDown ());
	}

	public bool AddSeconds (float amount) {
		if (!countingDown)
			return false;
		seconds += amount;
		return true;
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
