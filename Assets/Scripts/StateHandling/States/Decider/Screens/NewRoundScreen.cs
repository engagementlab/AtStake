using UnityEngine;
using System.Collections;

public class NewRoundScreen : GameScreen {

	LabelElement instructions = new LabelElement ("Please wait for other players to confirm they're ready :)", 1);
	bool allowContinue = false;

	public NewRoundScreen (GameState state, string name = "New Round") : base (state, name) {
		
		Events.instance.AddListener<MessagesMatchEvent> (OnMessagesMatchEvent);
		Events.instance.AddListener<RoundStartEvent> (OnRoundStartEvent);
		
		ScreenElements.AddEnabled ("instructions", instructions);
		ScreenElements.AddDisabled ("newDecider", new LabelElement ("You're the next Decider!", 0));
		ScreenElements.AddDisabled ("next", CreateBottomButton ("Next", "", "bottomPink", Side.Right));
	}

	public override void OnScreenStart (bool hosting, bool isDecider) {
		
		MessageMatcher.instance.SetMessage ("New Round", "true");

		if (Player.instance.Won) {
			DeciderSelectionManager.instance.SetDecider (Player.instance.Name);
			ScreenElements.Enable ("newDecider");
		} else {
			ScreenElements.Disable ("newDecider");
			ScreenElements.Disable ("next");
		}
	}

	protected override void OnButtonPress (ButtonPressEvent e) {
		if (e.id == "Next" && allowContinue) {
			GameStateController.instance.AllPlayersGotoScreen ("Bio", "Round");
			allowContinue = false;
		}
	}

	void OnMessagesMatchEvent (MessagesMatchEvent e) {
		if (e.id == "New Round") {
			instructions.Content = "Get ready!";
			if (Player.instance.Won) {
				ScreenElements.Enable ("next");
				allowContinue = true;
			} 
		}
	}

	void OnRoundStartEvent (RoundStartEvent e) {
		instructions.Content = "Please wait for other players to confirm they're ready :)";
	}
}
