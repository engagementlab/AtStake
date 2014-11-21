using UnityEngine;
using System.Collections;

public class GameScreen {

	public readonly string name = "";
	ScreenElement[] elements = new ScreenElement[0];
	public ScreenElement[] Elements {
		get { return elements; }
	}

	public GameScreen (string name) {
		this.name = name;
		Events.instance.AddListener<ButtonPressEvent> (OnButtonPressEvent);
	}

	public void SetElements (ScreenElement[] elements) {
		this.elements = elements;
	}

	public void GotoScreen (string screenName, string stateName = "") {
		GameStateController.instance.GotoScreen (screenName, stateName);
	}

	public virtual void OnButtonPressEvent (ButtonPressEvent e) {

	}
}
