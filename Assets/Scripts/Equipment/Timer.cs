using UnityEngine;
using System.Collections;

static public class TimerValues {

	static public readonly float brainstorm = 10f;
	static public readonly float pitch = 10f;
	static public readonly float deliberate = 10f;
	static public readonly float extraTime = 5f;

}

[RequireComponent (typeof (NetworkView))]
public class Timer : MonoBehaviour {

	float seconds = -1f;
	public float Seconds {
		get { return seconds; }
	}

	bool countingDown = false;

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

	public void AllStartCountDown (float duration) {
		networkView.RPC ("ReceiveCountDown", RPCMode.All, duration);
	}

	public void StartCountDown (float duration) {
		if (countingDown)
			return;
		seconds = duration;
		StartCoroutine (CountDown ());
	}

	public void AddSeconds (float amount) {
		if (!countingDown) {
			seconds = amount;
			StartCoroutine (CountDown ());
		} 
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
		Events.instance.Raise (new CountDownEndEvent ());
	}

	[RPC]
	void ReceiveCountDown (float duration) {
		StartCountDown (duration);
	}
}