using UnityEngine;
using System.Collections;

public class GameScreen {

	public readonly string name = "";
	public readonly GameState state;
	
	// deprecate this
	ScreenElement[] staticElements = new ScreenElement[0];
	public ScreenElement[] StaticElements {
		get { return staticElements; }
	}
	
	// deprecate this
	ScreenElement[] variableElements = new ScreenElement[0];
	public ScreenElement[] VariableElements {
		get { return variableElements; }
	}
	
	// deprecate this	
	ScreenElement[] elements = new ScreenElement[0];
	public ScreenElement[] Elements {
		get {
			if (ScreenElements.Empty) {
				return elements; 
			} else {
				return ScreenElements.Elements;
			}
		}
	}

	// New way of handling ScreenElements
	ScreenElements screenElements = new ScreenElements ();
	public ScreenElements ScreenElements { 
		get { return screenElements; }
		protected set {
			screenElements = value;
		}
	}

	public virtual TextAnchor Alignment {
		get { return TextAnchor.MiddleCenter; }
	}

	public GameScreen (GameState state, string name) {
		this.state = state;
		this.name = name;
		Events.instance.AddListener<ButtonPressEvent> (OnButtonPressEvent);
		Events.instance.AddListener<CountDownEndEvent> (OnCountDownEndEvent);
	}

	/**
	 *	Protected functions
	 */

	// Call this in the constructor to create elements that never change
	// deprecate this
	protected void SetStaticElements (ScreenElement[] staticElements) {
		if (this.staticElements.Length > 0) {
			Debug.LogWarning ("SetStaticElements() should only be called once per instance of GameScreen.");
		}
		this.staticElements = staticElements;
		RefreshElements ();
	}

	// Call this whenever to create & manipulate elements that move/change
	// deprecate this
	protected void SetVariableElements (ScreenElement[] variableElements) {
		this.variableElements = variableElements;
		RefreshElements ();
		Events.instance.Raise (new UpdateDrawerEvent ());
	}

	// deprecate this
	protected void AppendVariableElements (ScreenElement screenElement) {
		AppendVariableElements (new ScreenElement[] { screenElement });
	}

	// deprecate this
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

	// deprecate this
	protected void RemoveLastElement () {
		RemoveVariableElement (variableElements.Length-1);
	}

	// deprecate this
	protected void RemoveVariableElement (int index) {
		ScreenElement[] newElements = new ScreenElement[variableElements.Length];
		int count = 0;
		for (int i = 0; i < variableElements.Length; i ++) {
			if (i != index) {
				newElements[count] = variableElements[i];
				count ++;
			}
		}
		variableElements = newElements;
		RefreshElements ();
		Events.instance.Raise (new UpdateDrawerEvent ());	
	}

	protected void GotoScreen (string screenName, string stateName = "") {
		GameStateController.instance.GotoScreen (screenName, stateName);
	}

	protected ButtonElement CreateButton (string id, int position, string content="", string color="blue") {
		if (content == "")
			content = id;
		return new ButtonElement (this, id, content, position, color);
	}

	protected BottomButtonElement CreateBottomButton (string id, string content="", string color="", Side side=Side.Left) {
		if (content == "")
			content = id;
		return new BottomButtonElement (this, id, content, color, side);
	}

	protected TimerElement CreateTimer (string id, int position, string content="") {
		if (content == "")
			content = id;
		return new TimerElement (this, id, content, position);
	}

	protected void RefreshScreen () {
		OnScreenStart (MultiplayerManager.instance.Hosting, Player.instance.IsDecider);
	}

	// deprecate this
	protected void ClearScreen () {
		SetVariableElements (new ScreenElement[0]);
	}

	/**
	 *	Virtual functions
	 */

	// Host and clients hear this
	public virtual void OnScreenStart (bool hosting, bool isDecider) {

		ClearScreen ();
		
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

	public virtual void OnScreenEnd () {}

	// This function only gets called if the pressed button belongs to this GameScreen
	protected virtual void OnButtonPress (ButtonPressEvent e) {}

	public virtual void OnCountDownEnd () {}

	/**
	 *	Private functions
	 */

	// deprecate this
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

	// deprecate this
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

	void OnCountDownEndEvent (CountDownEndEvent e) {
		OnCountDownEnd ();
	}
}
