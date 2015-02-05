using UnityEngine;
using System.Collections;

public class LobbyScreen : GameScreen {
	
	bool hosting = false;
	int minPlayers = 2;		// the number of players that must join before we show the play button TODO: the screen drawer should probably handle this
	string[] playerNames = new string[0];

	public override TextAnchor Alignment {
		get { return TextAnchor.UpperCenter; }
	}

	public LobbyScreen (GameState state, string name = "Lobby") : base (state, name) {
		ScreenElements.AddEnabled ("title", new LabelElement ("Lobby", 0, new HeaderTextStyle ()));
		ScreenElements.AddEnabled ("back", CreateBottomButton ("Back"));
		Events.instance.AddListener<RefreshPlayerListEvent> (OnRefreshPlayerListEvent);
	}

	public override void OnScreenStart (bool hosting, bool isDecider) {}

	void OnRefreshPlayerListEvent (RefreshPlayerListEvent e) {
		
		playerNames = e.playerNames;
		hosting = MultiplayerManager.instance.Hosting;
		int namesCount = playerNames.Length;
		bool showPlay = hosting && namesCount > minPlayers;
		int elementCount = showPlay ? namesCount+1 : namesCount;

		for (int i = 0; i < namesCount; i ++) {
			ScreenElements.AddEnabled (playerNames[i], new LabelElement (playerNames[i], i+1));
		}
		if (showPlay) ScreenElements.AddEnabled ("play", CreateButton ("Play", elementCount+1));
	}

	protected override void OnButtonPress (ButtonPressEvent e) {
		switch (e.id) {
			case "Back": GoBack (); break;
			case "Play": PlayGame (); break;
		}
	}

	void GoBack () {
		MultiplayerManager.instance.ExitLobby ();
		GotoScreen (hosting ? "Host or Join" : "Games List");
	}

	void PlayGame () {
		MultiplayerManager.instance.StartGame ();
		GameStateController.instance.AllPlayersGotoScreen ("Choose Deck", "Decider");
	}
}
