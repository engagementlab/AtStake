using UnityEngine;
using System.Collections;

public class GameScreen {

	public readonly string name = "";

	ScreenElement[] staticElements = new ScreenElement[0];
	public ScreenElement[] StaticElements {
		get { return staticElements; }
	}

	ScreenElement[] variableElements = new ScreenElement[0];
	public ScreenElement[] VariableElements {
		get { return variableElements; }
	}
	
	ScreenElement[] elements = new ScreenElement[0];
	public ScreenElement[] Elements {
		get { return elements; }
	}

	public GameScreen (string name) {
		this.name = name;
		Events.instance.AddListener<ButtonPressEvent> (OnButtonPressEvent);
	}

	/**
	*	Public functions
	*/

	public void GotoScreen (string screenName, string stateName = "") {
		GameStateController.instance.GotoScreen (screenName, stateName);
	}

	// Call this in the constructor to create elements that never change
	public void SetStaticElements (ScreenElement[] staticElements) {
		if (this.staticElements.Length > 0) {
			Debug.LogWarning ("SetStaticElements() should only be called once per instance of GameScreen.");
		}
		this.staticElements = staticElements;
		RefreshElements ();
	}

	// Call this whenever to create & manipulate elements that move/change
	public void SetVariableElements (ScreenElement[] variableElements) {
		this.variableElements = variableElements;
		RefreshElements ();
		Events.instance.Raise (new UpdateDrawerEvent ());
	}

	/**
	*	Virtual functions
	*/

	public virtual void OnScreenStart () {}
	public virtual void OnButtonPressEvent (ButtonPressEvent e) {}

	/**
	*	Private functions
	*/

	void RefreshElements () {
		
		int sel = staticElements.Length;
		int vel = variableElements.Length;
		ScreenElement[] se = new ScreenElement[sel+vel];

		for (int i = 0; i < sel; i ++) {
			se[i] = staticElements[i];
		}

		for (int i = sel; i < sel+vel; i ++) {
			se[i] = variableElements[i-sel];
		}
		
		SetElements (se);
	}

	void SetElements (ScreenElement[] elements) {
		this.elements = elements;
	}
}
