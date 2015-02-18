using UnityEngine;
using System.Collections;

public class GameScreen {

	public readonly string name = "";
	public readonly GameState state;

	BottomButtonElement nextButton = null;
	bool HasNext {
		get { return (nextButton != null); }
	}

	ScreenElements screenElements = new ScreenElements ();
	public ScreenElements ScreenElements { 
		get { return screenElements; }
		protected set {
			screenElements = value;
		}
	}

	public ScreenElement[] Elements {
		get { return ScreenElements.Elements; }
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

	protected void GoBackScreen (string screenName) {
		GameStateController.instance.GotoScreen (screenName, "", true);
	}

	protected void GotoScreen (string screenName, string stateName="", bool back=false) {
		GameStateController.instance.GotoScreen (screenName, stateName, back);
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

	protected BottomButtonElement CreateNextButton () {
		nextButton = CreateBottomButton ("Next", "", "bottomPink", Side.Right);
		return nextButton;
	}

	protected TimerElement CreateTimer (string id, int position, string content="") {
		if (content == "")
			content = id;
		return new TimerElement (this, id, content, position);
	}

	protected void RefreshScreen () {
		OnScreenStart (MultiplayerManager.instance.Hosting, Player.instance.IsDecider);
	}

	/**
	 *	Virtual functions
	 */

	// Host and clients hear this
	public virtual void OnScreenStart (bool hosting, bool isDecider) {
		if (HasNext) {
			Events.instance.AddListener<MessagesMatchEvent> (OnMessagesMatchEvent);
			nextButton.Content = "Next";
		}
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

	public virtual void OnScreenEnd () {
		Events.instance.RemoveListener<MessagesMatchEvent> (OnMessagesMatchEvent);
	}

	/**
	 *	Messages
	 */

	protected virtual void OnButtonPressEvent (ButtonPressEvent e) {
		if (e.screen == this) {
			if (e.id == "Next" && HasNext) {
				nextButton.Content = "Wait";
				MessageMatcher.instance.SetMessage (name + " next", "next screen");
			}
			OnButtonPress (e);
		}
	}

	void OnCountDownEndEvent (CountDownEndEvent e) {
		OnCountDownEnd ();
	}

	protected virtual void OnMessagesMatchEvent (MessagesMatchEvent e) {
		if (e.id == name + " next") {
			OnMessagesMatch ();
		}
	}

	protected virtual void OnMessagesMatch () {
		GameStateController.instance.GotoNextScreen ();
	}
}
