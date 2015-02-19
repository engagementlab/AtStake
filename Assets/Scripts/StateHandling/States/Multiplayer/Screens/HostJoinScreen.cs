using UnityEngine;
using System.Collections;

public class HostJoinScreen : GameScreen {

	public HostJoinScreen (GameState state, string name = "Host or Join") : base (state, name) {
		ScreenElements.AddEnabled ("copy", new LabelElement ("Select host or join", 0));
		ScreenElements.AddEnabled ("host", CreateButton ("Host", 2));
		ScreenElements.AddEnabled ("join", CreateButton ("Join", 3, "", "green"));
		ScreenElements.AddDisabled ("searching", new LabelElement ("Searching for games...", 4));
		ScreenElements.AddDisabled ("nogames", new LabelElement ("No games found.", 4));
		ScreenElements.AddEnabled ("back", CreateBottomButton ("Back"));

		Events.instance.AddListener<JoinTimeoutEvent> (OnJoinTimeoutEvent);
		Events.instance.AddListener<FoundGamesEvent> (OnFoundGamesEvent);
	}

	public override void OnScreenStart (bool hosting, bool isDecider) {
		ScreenElements.Disable ("searching");
		ScreenElements.Disable ("nogames");
	}

	protected override void OnButtonPress (ButtonPressEvent e) {
		switch (e.id) {
			case "Host": 
				MultiplayerManager.instance.HostGame (); 
				GotoScreen ("Lobby");
				break;
			case "Join": 
				MultiplayerManager.instance.JoinGame (); 
				ScreenElements.Enable ("searching");
				ScreenElements.Disable ("nogames");
				break;
			case "Back": 
				GoBackScreen ("Enter Name"); 
				break;
		}
	}

	void OnJoinTimeoutEvent (JoinTimeoutEvent e) {
		ScreenElements.Disable ("searching");
		ScreenElements.Enable ("nogames");
	}

	void OnFoundGamesEvent (FoundGamesEvent e) {
		ScreenElements.Disable ("searching");
		ScreenElements.Disable ("nogames");
	}
}
