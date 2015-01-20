using UnityEngine;
using System.Collections;

public class LobbyScreen : GameScreen {
	
	LabelElement label;
	bool hosting = false;
	int minPlayers = 2;		// the number of players that must join before we show the play button TODO: the screen drawer should probably handle this
	string[] playerNames = new string[0];

	public LobbyScreen (GameState state, string name = "Lobby") : base (state, name) {
		SetStaticElements (new ScreenElement[] {
			new LabelElement ("Lobby", 0),
			CreateBottomButton ("Back")
		});
		Events.instance.AddListener<RefreshPlayerListEvent> (OnRefreshPlayerListEvent);
	}

	public override void OnScreenStart (bool hosting, bool isDecider) {}

	void OnRefreshPlayerListEvent (RefreshPlayerListEvent e) {
		
		playerNames = e.playerNames;

		hosting = MultiplayerManager.instance.Hosting;

		int namesCount = playerNames.Length;
		bool showPlay = hosting && namesCount > minPlayers;
		int elementCount = showPlay ? namesCount+1 : namesCount;
		ScreenElement[] se = new ScreenElement[elementCount];

		for (int i = 0; i < namesCount; i ++) {
			se[i] = new LabelElement (playerNames[i], i+1);
		}

		if (showPlay) se[elementCount-1] = CreateButton ("Play", elementCount+1);

		SetVariableElements (se);
	}

	protected override void OnButtonPress (ButtonPressEvent e) {
		switch (e.id) {
			case "Back": GoBack (); break;
			case "Play": GotoChooseDeck (); break;
		}
	}

	void GoBack () {
		MultiplayerManager.instance.ExitLobby ();
		GotoScreen (hosting ? "Host or Join" : "Games List");
	}

	void GotoChooseDeck () {
		GameStateController.instance.AllPlayersGotoScreen ("Choose Deck", "Decider");
	}
}
