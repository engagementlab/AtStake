using UnityEngine;
using System.Collections;

public class SlideController : MonoBehaviour {

	public TopBar topBar;
	public UIScreen1 screen1;
	public UIScreen2 screen2;
	UIScreen currentScreen;
	UIScreen nextScreen;

	static float slideTime = 0.33f;
	public static float SlideTime {
		get { return slideTime; }
	}

	static bool sliding = false; 
	public static bool Sliding {
		get { return sliding; }
	}

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
		topBar.OnChangeScreenEvent (e);
	}

	void OnUpdateDrawerEvent (UpdateDrawerEvent e) {
		currentScreen.OnUpdateDrawerEvent (e);
		topBar.OnUpdateDrawerEvent (e);
	}
}