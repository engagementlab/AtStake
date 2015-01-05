using UnityEngine;
using System.Collections;

public class NewRoundScreen : GameScreen {

	LabelElement instructions = new LabelElement ("Please wait for other players to confirm they're ready :)");
	int confirmCount = 0;
	bool allowContinue = false;

	public NewRoundScreen (GameState state, string name = "New Round") : base (state, name) {
		Events.instance.AddListener<HostReceiveMessageEvent> (OnHostReceiveMessageEvent);
		Events.instance.AddListener<AllReceiveMessageEvent> (OnAllReceiveMessageEvent);
		Events.instance.AddListener<MessagesMatchEvent> (OnMessagesMatchEvent);
		SetStaticElements (new ScreenElement[] {
			instructions
		});
	}

	public override void OnScreenStart (bool hosting, bool isDecider) {
		MessageMatcher.instance.SetMessage ("New Round", "true");

		if (Player.instance.Won) {
			DeciderSelectionManager.instance.SetDecider (Player.instance.Name);
			SetVariableElements (new ScreenElement[] {
				new LabelElement ("You're the next Decider!"),
				CreateButton ("Next")
			});
		}
	}

	protected override void OnButtonPress (ButtonPressEvent e) {
		if (e.id == "Next" && allowContinue) {
			GameStateController.instance.AllPlayersGotoScreen ("Introduction", "Round");
			confirmCount = 0;
			allowContinue = false;
		}
	}

	void AddConfirmation () {
		confirmCount ++;
		if (confirmCount == MultiplayerManager.instance.PlayerCount) {
			MessageRelayer.instance.SendMessageToAll ("AllowContinue");
		}
	}

	void OnHostReceiveMessageEvent (HostReceiveMessageEvent e) {
		if (e.id == "ConfirmNewRound") {
			AddConfirmation ();
		}
	}

	void OnAllReceiveMessageEvent (AllReceiveMessageEvent e) {
		if (e.id == "AllowContinue" && Player.instance.IsDecider) {
			allowContinue = true;
		}
	}

	void OnMessagesMatchEvent (MessagesMatchEvent e) {
		if (e.id == "New Round") {
			instructions.content = "Get ready!";
			allowContinue = true;
		}
	}
}
