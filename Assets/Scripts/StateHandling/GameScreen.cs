using UnityEngine;
using System.Collections;

public class GameScreen {

	public readonly string name = "";
	public readonly GameState state;

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

	public GameScreen (GameState state, string name) {
		this.state = state;
		this.name = name;
		Events.instance.AddListener<ButtonPressEvent> (OnButtonPressEvent);
	}

	/**
	*	Protected functions
	*/

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

	protected void AppendVariableElements (ScreenElement screenElement) {
		AppendVariableElements (new ScreenElement[] { screenElement });
	}

	protected void AppendVariableElements (ScreenElement[] appendedElements) {
		
		ScreenElement[] tempElements = variableElements;
		ScreenElement[] newVariableElements = new ScreenElement[tempElements.Length + appendedElements.Length];
		for (int i = 0; i < tempElements.Length; i ++) {
			newVariableElements[i] = tempElements[i];
		}

		int index = 0;
		for (int i = tempElements.Length; i < newVariableElements.Length; i ++) {
			newVariableElements[i] = appendedElements[index];
			index ++;
		}

		variableElements = newVariableElements;
		RefreshElements ();
		Events.instance.Raise (new UpdateDrawerEvent ());
	}

	protected void GotoScreen (string screenName, string stateName = "") {
		GameStateController.instance.GotoScreen (screenName, stateName);
	}

	protected ButtonElement CreateButton (string id, string content="") {
		if (content == "")
			content = id;
		return new ButtonElement (this, id, content);
	}

	protected TimerElement CreateTimer (float startTime) {
		return new TimerElement (this, startTime);
	}

	/**
	*	Virtual functions
	*/

	// Host and clients hear this
	public virtual void OnScreenStart (bool hosting, bool isDecider) {
		if (hosting) {
			OnScreenStartHost ();
		} else {
			OnScreenStartClient ();
		}
		if (isDecider) {
			OnScreenStartDecider ();
		} else {
			OnScreenStartPlayer ();
		}
	}

	// Only the host hears this
	protected virtual void OnScreenStartHost () {}

	// Only clients hear this
	protected virtual void OnScreenStartClient () {}

	// Only the decider hears this
	protected virtual void OnScreenStartDecider () {}

	// Everyone but the decider hears this
	protected virtual void OnScreenStartPlayer () {}

	// This function only gets called if the pressed button belongs to this GameScreen
	protected virtual void OnButtonPress (ButtonPressEvent e) {}

	public virtual void OnCountDownEnd () {}

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
