using UnityEngine;
using System.Collections;

public class GamesListScreen : GameScreen {

	HostData[] hosts;
	LabelElement nameTaken = new LabelElement ("");

	public GamesListScreen (GameState state, string name = "Games List") : base (state, name) {
		Events.instance.AddListener<FoundGamesEvent> (OnFoundGamesEvent);
		Events.instance.AddListener<RegisterEvent> (OnRegisterEvent);
		Events.instance.AddListener<NameTakenEvent> (OnNameTakenEvent);
		SetStaticElements (new ScreenElement[] {
			new LabelElement ("Choose a game to join"),
			CreateButton ("Back"),
			nameTaken
		});
	}

	public override void OnScreenStart (bool hosting, bool isDecider) {}

	void OnFoundGamesEvent (FoundGamesEvent e) {
		hosts = e.hosts;
		ScreenElement[] se = new ScreenElement[hosts.Length];
		for (int i = 0; i < se.Length; i ++) {
			string gameName = hosts[i].gameName;
			se[i] = CreateButton (i.ToString () + "__" + gameName, gameName);
		}
		SetVariableElements (se);
	}

	protected override void OnButtonPress (ButtonPressEvent e) {
		switch (e.id) {
			case "Back": GotoScreen ("Host or Join"); break;
		}
		char c = e.id[0];
		int n = (int)char.GetNumericValue (c);
		if (n > -1) {
			MultiplayerManager.instance.ConnectToHost (hosts[n]);
		}
	}

	void OnRegisterEvent (RegisterEvent e) {
		GotoScreen ("Lobby");
	}

	void OnNameTakenEvent (NameTakenEvent e) {
		nameTaken.content = string.Format ("There's already someone named {0} in this game. Please go back and choose a different name", Player.instance.Name);
	}
}
