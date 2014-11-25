using UnityEngine;
using System.Collections;

public class LobbyScreen : GameScreen {
	
	LabelElement label;
	bool hosting = false;
	int minPlayers = 1;		// the number of players that must join before we show the play button TODO: the screen drawer should probably handle this

	public LobbyScreen (string name = "Lobby") : base (name) {
		SetStaticElements (new ScreenElement[] {
			new LabelElement ("Lobby"),
			new ButtonElement ("Back-Lobby", "Back")
		});
		Events.instance.AddListener<RefreshPlayerListEvent> (OnRefreshPlayerListEvent);
	}

	void OnRefreshPlayerListEvent (RefreshPlayerListEvent e) {

		hosting = MultiplayerManager.instance.Hosting;

		string[] names = e.playerNames;
		int namesCount = names.Length;
		bool showPlay = hosting && namesCount > minPlayers;
		int elementCount = showPlay ? namesCount+1 : namesCount;
		ScreenElement[] se = new ScreenElement[elementCount];

		for (int i = 0; i < namesCount; i ++) {
			se[i] = new LabelElement (names[i]);
		}

		if (showPlay) se[elementCount-1] = new ButtonElement ("Play-Lobby", "Play");

		SetVariableElements (se);
	}

	public override void OnButtonPressEvent (ButtonPressEvent e) {
		switch (e.id) {
			case "Back-Lobby": GoBack (); break;
			case "Play-Lobby": Debug.Log ("play!"); break;
		}
	}

	void GoBack () {
		MultiplayerManager.instance.ExitLobby ();
		GotoScreen (hosting ? "Host or Join" : "Games List");
	}
}
