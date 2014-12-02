using UnityEngine;
using System.Collections;

public class Timer : MonoBehaviour {

	float seconds = 0f;
	public float Seconds {
		get { return seconds; }
	}

	bool countingDown = false;

	void Start () {

	}

	void StartCountDown (float duration) {
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
		
	}
}
