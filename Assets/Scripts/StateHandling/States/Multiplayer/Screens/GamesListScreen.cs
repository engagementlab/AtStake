using UnityEngine;
using System.Collections;

public class GamesListScreen : GameScreen {

	HostData[] hosts;
	LabelElement nameTaken = new LabelElement ("", 1);

	public GamesListScreen (GameState state, string name = "Games List") : base (state, name) {
		Events.instance.AddListener<FoundGamesEvent> (OnFoundGamesEvent);
		Events.instance.AddListener<RegisterEvent> (OnRegisterEvent);
		Events.instance.AddListener<NameTakenEvent> (OnNameTakenEvent);
		ScreenElements.AddEnabled ("copy", new LabelElement ("Choose a game to join", 0));
		ScreenElements.AddDisabled ("nameTaken", nameTaken);
		ScreenElements.AddEnabled ("back", CreateBottomButton ("Back"));
		ScreenElements.Disable ("nameTaken");
	}

	public override void OnScreenStart (bool hosting, bool isDecider) {}

	void OnFoundGamesEvent (FoundGamesEvent e) {
		hosts = e.hosts;
		for (int i = 0; i < hosts.Length; i ++) {
			string gameName = hosts[i].gameName;
			ScreenElements.AddEnabled (gameName, CreateButton (i.ToString () + "__" + gameName, i+2, gameName));
		}
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
		nameTaken.Content = string.Format ("There's already someone named {0} in this game. Please go back and choose a different name", Player.instance.Name);
		ScreenElements.Enable ("nameTaken");
	}
}
