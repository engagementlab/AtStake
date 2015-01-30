using UnityEngine;
using System.Collections;

public static class TimerValues {

	public static readonly float brainstorm = 1f;
	public static readonly float pitch = 1f;
	public static readonly float deliberate = 1f;
	public static readonly float extraTime = 0.5f;

}

[RequireComponent (typeof (NetworkView))]
public class Timer : MonoBehaviour {

	float duration = 0;
	float seconds = -1;
	public float Seconds {
		get { return seconds; }
	}

	public float Progress {
		get { return seconds / duration; }
	}

	bool countingDown = false;
	public bool CountingDown {
		get { return countingDown; }
	}

	static public Timer instance;

	void Awake () {
		if (instance == null)
			instance = this;
		Events.instance.AddListener<ChangeScreenEvent> (OnChangeScreenEvent);
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
		this.duration = duration;
		seconds = duration;
		StartCoroutine (CountDown ());
	}

	public void AllAddSeconds (float amount) {
		networkView.RPC ("ReceiveAddSeconds", RPCMode.All, amount);
	}

	void AddSeconds (float amount) {
		if (!countingDown) {
			duration = amount;
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

	void OnChangeScreenEvent (ChangeScreenEvent e) {
		if (!countingDown)
			seconds = -1;
	}

	[RPC]
	void ReceiveCountDown (float duration) {
		StartCountDown (duration);
	}

	[RPC]
	void ReceiveAddSeconds (float amount) {
		AddSeconds (amount);
	}
}