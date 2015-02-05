using UnityEngine;
using System.Collections;

public class HostJoinScreen : GameScreen {

	public HostJoinScreen (GameState state, string name = "Host or Join") : base (state, name) {
		ScreenElements.AddEnabled ("copy", new LabelElement ("Select host or join", 0));
		ScreenElements.AddDisabled ("searching", new LabelElement ("searching for games...", 1));
		ScreenElements.AddDisabled ("nogames", new LabelElement ("no games found :(", 2));
		ScreenElements.AddEnabled ("host", CreateButton ("Host", 3));
		ScreenElements.AddEnabled ("join", CreateButton ("Join", 4, "", "green"));
		ScreenElements.AddEnabled ("back", CreateBottomButton ("Back"));

		Events.instance.AddListener<JoinTimeoutEvent> (OnJoinTimeoutEvent);
		Events.instance.AddListener<FoundGamesEvent> (OnFoundGamesEvent);
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
				break;
			case "Back": 
				GotoScreen ("Enter Name"); 
				break;
		}
	}

	void OnJoinTimeoutEvent (JoinTimeoutEvent e) {
		ScreenElements.Disable ("searching");
		ScreenElements.Enable ("nogames");
	}

	void OnFoundGamesEvent (FoundGamesEvent e) {
		ScreenElements.Disable ("nogames");
	}
}
