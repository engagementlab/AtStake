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
	*	Protected functions
	*/

	protected void GotoScreen (string screenName, string stateName = "") {
		GameStateController.instance.GotoScreen (screenName, stateName);
	}

	// Call this in the constructor to create elements that never change
	protected void SetStaticElements (ScreenElement[] staticElements) {
		if (this.staticElements.Length > 0) {
			Debug.LogWarning ("SetStaticElements() should only be called once per instance of GameScreen.");
		}
		this.staticElements = staticElements;
		RefreshElements ();
	}

	// Call this whenever to create & manipulate elements that move/change
	protected void SetVariableElements (ScreenElement[] variableElements) {
		this.variableElements = variableElements;
		RefreshElements ();
		Events.instance.Raise (new UpdateDrawerEvent ());
	}

	protected ButtonElement CreateButton (string id, string content="") {
		if (content == "")
			content = id;
		return new ButtonElement (this, id, content);
	}

	/**
	*	Virtual functions
	*/

	// Host and clients hear this
	public virtual void OnScreenStart () {
		if (MultiplayerManager.instance.Hosting) {
			OnScreenStartHost ();
		} else {
			OnScreenStartClient ();
		}
	}

	// Only the host hears this
	public virtual void OnScreenStartHost () {}

	// Only clients hear this
	public virtual void OnScreenStartClient () {}

	// This function only gets called if the pressed button belongs to this GameScreen
	public virtual void OnButtonPress (ButtonPressEvent e) {}

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

	/**
	*	Messages
	*/

	void OnButtonPressEvent (ButtonPressEvent e) {
		if (e.screen == this)
			OnButtonPress (e);
	}
}
