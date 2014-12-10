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

	public void StartCountDown (TimerElement timerElement, float duration) {
		this.timerElement = timerElement;
		seconds = duration;
		StartCoroutine (CountDown ());
	}

	void AddSeconds (float amount) {
		if (!countingDown) {
			seconds = amount;
			StartCoroutine (CountDown ());
		}
		seconds += amount;
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

	public void DeciderAddSeconds (float amount) {
		networkView.RPC ("AddSecondsMessage", RPCMode.Others, amount);
	}

	[RPC]
	void AddSecondsMessage (float amount) {
		if (Player.instance.IsDecider) {
			AddSeconds (amount);
		}
	}
}
