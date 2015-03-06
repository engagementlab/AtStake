using UnityEngine;
using System.Collections;

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
		Events.instance.AddListener<AllReceiveMessageEvent> (OnAllReceiveMessageEvent);
	}

	public void SetTime (float seconds) {
		if (!countingDown) {
			this.seconds = seconds;
		}
	}

	public void AllStartCountDown (float duration) {
		MessageSender.instance.SendMessageToAll ("ReceiveCountDown", duration.ToString ());
	}

	public void StartCountDown (float duration) {
		if (countingDown)
			return;
		this.duration = duration;
		seconds = duration;
		StartCoroutine (CountDown ());
	}

	public void AllAddSeconds (float amount) {
		MessageSender.instance.SendMessageToAll ("ReceiveAddSeconds", amount.ToString ());
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

	bool SendRPC (string name, RPCMode mode, params object[] args) {
		if (Network.isClient || Network.isServer) {
			networkView.RPC (name, mode, args);
			return true;
		}
		return false;
	}

	void OnAllReceiveMessageEvent (AllReceiveMessageEvent e) {
		if (e.id == "ReceiveAddSeconds") {
			ReceiveAddSeconds (float.Parse (e.message1));
		} else if (e.id == "ReceiveCountDown") {
			ReceiveCountDown (float.Parse (e.message1));
		}
	}

	void ReceiveCountDown (float duration) {
		StartCountDown (duration);
	}

	void ReceiveAddSeconds (float amount) {
		AddSeconds (amount);
	}
}