using UnityEngine;
using System.Collections;

public class SlideController : MonoBehaviour {

	public UIScreen1 screen1;
	public UIScreen2 screen2;
	UIScreen currentScreen;
	UIScreen nextScreen;

	float slideTime = 0.33f;
	bool sliding = false;

	bool firstScreen = true;

	void Awake () {
		Events.instance.AddListener<ChangeScreenEvent> (OnChangeScreenEvent);
		Events.instance.AddListener<UpdateDrawerEvent> (OnUpdateDrawerEvent);
		currentScreen = screen1;
		nextScreen = screen2;
	}

	IEnumerator SlideLeft () {
		while (sliding) {
			yield return null;
		}
		currentScreen.SlideLeft (slideTime);
		nextScreen.SlideLeft (slideTime);
		StartSlide ();
		Invoke ("EndSlide", slideTime);
	}

	IEnumerator SlideRight () {
		while (sliding) {
			yield return null;
		}
		currentScreen.SlideRight (slideTime);
		nextScreen.SlideRight (slideTime);
		StartSlide ();
		Invoke ("EndSlide", slideTime);
	}

	void StartSlide () {
		sliding = true;
		UIScreen tempScreen = currentScreen;
		currentScreen = nextScreen;
		nextScreen = tempScreen;
	}

	void EndSlide () {
		sliding = false;
	}

	void OnChangeScreenEvent (ChangeScreenEvent e) {
		if (firstScreen) {
			currentScreen.OnChangeScreenEvent (e);
			firstScreen = false;
		} else {
			nextScreen.OnChangeScreenEvent (e);
			if (e.back) {
				StartCoroutine (SlideRight ());
			} else {
				StartCoroutine (SlideLeft ());
			}
		}
	}

	void OnUpdateDrawerEvent (UpdateDrawerEvent e) {
		currentScreen.OnUpdateDrawerEvent (e);
	}
}