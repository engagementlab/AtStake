using UnityEngine;
using System.Collections;

public class GamesListScreen : GameScreen {

	HostData[] hosts;

	public GamesListScreen (GameState state, string name = "Games List") : base (state, name) {
		Events.instance.AddListener<FoundGamesEvent> (OnFoundGamesEvent);
		Events.instance.AddListener<NameTakenEvent> (OnNameTakenEvent);
		ScreenElements.AddEnabled ("copy", new LabelElement ("Choose a game to join", 0));
		ScreenElements.AddEnabled ("back", CreateBottomButton ("Back"));
	}

	public override void OnScreenStart (bool hosting, bool isDecider) {
		Events.instance.AddListener<RegisterEvent> (OnRegisterEvent);
	}

	public override void OnScreenEnd () {
		base.OnScreenEnd ();
		Events.instance.RemoveListener<RegisterEvent> (OnRegisterEvent);
	}

	void OnFoundGamesEvent (FoundGamesEvent e) {
		ScreenElements.SuspendUpdating ();
		hosts = e.hosts;
		for (int i = 0; i < hosts.Length; i ++) {
			string gameName = hosts[i].gameName;
			string id = "game" + i.ToString ();
			ScreenElements.Remove (id);
			ScreenElements.AddEnabled (id, CreateButton (i.ToString () + "__" + gameName, i+2, gameName));
		}
		ScreenElements.EnableUpdating ();
	}

	protected override void OnButtonPress (ButtonPressEvent e) {
		switch (e.id) {
			case "Back": GoBackScreen ("Host or Join"); break;
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
		GotoScreen ("Name Taken");
	}
}
