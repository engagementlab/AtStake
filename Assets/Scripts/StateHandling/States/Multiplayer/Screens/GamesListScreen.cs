using UnityEngine;
using System.Collections;

public class GamesListScreen : GameScreen {

	HostData[] hosts;

	public GamesListScreen (string name = "Games List") : base (name) {
		Events.instance.AddListener<FoundGamesEvent> (OnFoundGamesEvent);
	}

	void OnFoundGamesEvent (FoundGamesEvent e) {
		hosts = e.hosts;
		ScreenElement[] se = new ScreenElement[hosts.Length];
		for (int i = 0; i < se.Length; i ++) {
			string gameName = hosts[i].gameName;
			se[i] = new ButtonElement (i.ToString () + "__" + gameName, gameName);
		}
		SetElements (se);
		Events.instance.Raise (new UpdateDrawerEvent ());
	}

	public override void OnButtonPressEvent (ButtonPressEvent e) {
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
