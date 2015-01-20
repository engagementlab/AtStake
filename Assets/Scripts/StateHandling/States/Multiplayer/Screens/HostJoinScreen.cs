using UnityEngine;
using System.Collections;

public class HostJoinScreen : GameScreen {

	LabelElement label;
	string defaultText = "Select host or join";

	public HostJoinScreen (GameState state, string name = "Host or Join") : base (state, name) {
		label = new LabelElement (defaultText, 0);
		SetStaticElements (new ScreenElement[] {
			label,
			CreateButton ("Host", 1),
			CreateButton ("Join", 2),
			CreateBottomButton ("Back")
		});

		Events.instance.AddListener<JoinTimeoutEvent> (OnJoinTimeoutEvent);
		Events.instance.AddListener<FoundGamesEvent> (OnFoundGamesEvent);
	}

	public override void OnScreenStart (bool hosting, bool isDecider) {
		label.Content = defaultText;
	}

	protected override void OnButtonPress (ButtonPressEvent e) {
		switch (e.id) {
			case "Host": 
				MultiplayerManager.instance.HostGame (); 
				GotoScreen ("Lobby");
				break;
			case "Join": 
				MultiplayerManager.instance.JoinGame (); 
				label.Content = "searching for games...";
				break;
			case "Back": 
				GotoScreen ("Enter Name"); 
				break;
		}
	}

	void OnJoinTimeoutEvent (JoinTimeoutEvent e) {
		label.Content = "no games found :(";
	}

	void OnFoundGamesEvent (FoundGamesEvent e) {
		label.Content = "";
	}
}
