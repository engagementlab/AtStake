using UnityEngine;
using System.Collections;

public class NewRoundScreen : GameScreen {

	LabelElement instructions = new LabelElement ("Please wait for other players to confirm they're ready :)", 0);
	bool allowContinue = false;

	public NewRoundScreen (GameState state, string name = "New Round") : base (state, name) {
		Events.instance.AddListener<MessagesMatchEvent> (OnMessagesMatchEvent);
		Events.instance.AddListener<RoundStartEvent> (OnRoundStartEvent);
		SetStaticElements (new ScreenElement[] {
			instructions
		});
	}

	public override void OnScreenStart (bool hosting, bool isDecider) {
		
		MessageMatcher.instance.SetMessage ("New Round", "true");

		ClearScreen ();
		if (Player.instance.Won) {
			DeciderSelectionManager.instance.SetDecider (Player.instance.Name);
			SetVariableElements (new ScreenElement[] {
				new LabelElement ("You're the next Decider!", 0)
			});
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
			Debug.Log("decider: " + Player.instance.IsDecider);
			Debug.Log("won: " + Player.instance.Won);
			if (Player.instance.IsDecider && Player.instance.Won) {
				AppendVariableElements (CreateBottomButton ("Next", "", "bottomPink", Side.Right));
				Debug.Log("heard");
				allowContinue = true;
			}
		}
	}

	void OnRoundStartEvent (RoundStartEvent e) {
		instructions.Content = "Please wait for other players to confirm they're ready :)";
	}
}
