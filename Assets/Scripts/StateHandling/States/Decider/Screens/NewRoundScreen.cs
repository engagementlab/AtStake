using UnityEngine;
using System.Collections;

public class NewRoundScreen : GameScreen {

	int confirmCount = 0;
	bool allowContinue = false;

	public NewRoundScreen (GameState state, string name = "New Round") : base (state, name) {
		Events.instance.AddListener<HostReceiveMessageEvent> (OnHostReceiveMessageEvent);
		Events.instance.AddListener<AllReceiveMessageEvent> (OnAllReceiveMessageEvent);
		string instructions = "Please wait for other players to confirm they're ready :)";
		SetStaticElements (new ScreenElement[] {
			new LabelElement (instructions)
		});
	}

	public override void OnScreenStart (bool hosting, bool isDecider) {
		Events.instance.Raise (new RoundEndEvent ());
		if (MultiplayerManager.instance.Hosting) {
			AddConfirmation ();
		} else {
			MessageRelayer.instance.SendMessageToHost ("ConfirmNewRound");
		}
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
}
