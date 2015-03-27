using UnityEngine;
using System.Collections;

public enum Side { Left, Right }

public class BottomButtonElement : ButtonElement {

	Side side = Side.Left;
	public Side Side {
		get { return side; }
	}

	public override string Content {
		get { return content.ToLower (); }
		set { 
			content = value.ToLower ();
			if (middleButton != null) 
				middleButton.text.text = content;

			// special case for "wait": animate ellipsis
			if (content == "wait") {
				StartWaitAnimation ();
			} 
		}
	}

	string MiddleButtonText {
		get { return middleButton.text.text; }
		set { middleButton.text.text = value; }
	}

	float animationTime = 1f;
	bool animating = false;
	GameScreen currentScreen;

	public BottomButtonElement (GameScreen screen, string id, string content, string color="bottomOrange", Side side=Side.Left) : base (screen, id, content, 0, color) {
		this.side = side;
		Content = content;
		Events.instance.AddListener<ChangeScreenEvent> (OnChangeScreenEvent);
	}

	void StartWaitAnimation () {
		if (!animating) {
			animating = true;			
			CoroutineManager.Instance.StartCoroutine (animationTime, WaitEllipsis);
		}
	}

	void WaitEllipsis (float progress) {
		float interval = 0.25f;
		if (progress >= interval && MiddleButtonText == "wait") {
			MiddleButtonText = "wait.";
		}
		if (progress >= interval*2 && MiddleButtonText == "wait.") {
			MiddleButtonText = "wait..";
		}
		if (progress >= interval*3 && MiddleButtonText == "wait..") {
			MiddleButtonText = "wait...";
		}
		if (progress >= interval*4) {
			MiddleButtonText = "wait";
			animating = false;
			if (Content == "wait" && currentScreen == screen) {
				Content = "wait";
			}
		}
	}

	void OnChangeScreenEvent (ChangeScreenEvent e) {
		currentScreen = e.screen;
	}
}
