using UnityEngine;
using System.Collections;

public class GamesListScreen : GameScreen {

	HostData[] hosts;

	public GamesListScreen (GameState state, string name = "Games List") : base (state, name) {
		Events.instance.AddListener<FoundGamesEvent> (OnFoundGamesEvent);
		SetStaticElements (new ScreenElement[] {
			new LabelElement ("Choose a game to join"),
			CreateButton ("Back")
		});
	}

	void OnFoundGamesEvent (FoundGamesEvent e) {
		hosts = e.hosts;
		ScreenElement[] se = new ScreenElement[hosts.Length];
		for (int i = 0; i < se.Length; i ++) {
			string gameName = hosts[i].gameName;
			se[i] = CreateButton (i.ToString () + "__" + gameName, gameName);
		}
		SetVariableElements (se);
	}

	public override void OnButtonPress (ButtonPressEvent e) {
		switch (e.id) {
			case "Back": GotoScreen ("Host or Join"); break;
		}
		char c = e.id[0];
		char c2 = e.id[1];
		int n = (int)char.GetNumericValue (c);
		int n2 = (int)char.GetNumericValue (c2);
		if (n > -1) {
			MultiplayerManager.instance.ConnectToHost (hosts [n]);
			GotoScreen ("Lobby");
		}
	}
}
