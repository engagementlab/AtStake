using UnityEngine;
using System.Collections;

public class SlideController : MonoBehaviour {

	public UIScreen1 screen1;
	public UIScreen2 screen2;
	UIScreen currentScreen;
	UIScreen nextScreen;

	float slideTime = 0.33f;

	bool firstScreen = true;

	void Awake () {
		Events.instance.AddListener<ChangeScreenEvent> (OnChangeScreenEvent);
		Events.instance.AddListener<UpdateDrawerEvent> (OnUpdateDrawerEvent);
		currentScreen = screen1;
		nextScreen = screen2;
	}

	void SlideLeft () {
		currentScreen.SlideLeft (slideTime);
		nextScreen.SlideLeft (slideTime);
		Invoke ("EndSlide", slideTime);
	}

	void SlideRight () {
		currentScreen.SlideRight (slideTime);
		nextScreen.SlideRight (slideTime);
		Invoke ("EndSlide", slideTime);
	}

	void EndSlide () {
		UIScreen tempScreen = currentScreen;
		currentScreen = nextScreen;
		nextScreen = tempScreen;
	}

	void OnChangeScreenEvent (ChangeScreenEvent e) {
		if (firstScreen) {
			currentScreen.OnChangeScreenEvent (e);
			firstScreen = false;
		} else {
			nextScreen.OnChangeScreenEvent (e);
			if (e.back) {
				SlideRight ();
			} else {
				SlideLeft ();
			}
		}
	}

	void OnUpdateDrawerEvent (UpdateDrawerEvent e) {
		currentScreen.OnUpdateDrawerEvent (e);
	}
}