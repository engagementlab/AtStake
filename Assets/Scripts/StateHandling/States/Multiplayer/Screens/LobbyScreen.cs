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
		ScreenElements.AddDisabled ("invite", CreateButton ("Invite More", 6));
		ScreenElements.AddDisabled ("play", CreateBottomButton ("Play", "", "bottomPink", Side.Right));
		Events.instance.AddListener<RefreshPlayerListEvent> (OnRefreshPlayerListEvent);
	}

	public override void OnScreenStart (bool hosting, bool isDecider) {
		Events.instance.AddListener<DisconnectedFromServerEvent> (OnDisconnectedFromServerEvent);
		if (!MultiplayerManager.instance.UsingWifi) {
			if (MultiplayerManager.instance.Hosting) {
				ScreenElements.Enable ("invite");
			} else {
				ScreenElements.Disable ("invite");
			}
		}
	}

	public override void OnScreenEnd () {
		base.OnScreenEnd ();
		Events.instance.RemoveListener<DisconnectedFromServerEvent> (OnDisconnectedFromServerEvent);
		if (!MultiplayerManager.instance.UsingWifi && MultiplayerManager.instance.Hosting) {
			ScreenElements.Disable ("invite");
		}
	}

	void OnRefreshPlayerListEvent (RefreshPlayerListEvent e) {
		
		playerNames = e.playerNames;
		hosting = MultiplayerManager.instance.Hosting;
		int namesCount = playerNames.Length;
		bool showPlay = hosting && namesCount > minPlayers;

		ScreenElements.SuspendUpdating ();
		for (int i = 0; i < 5; i ++) {
			string id = "name" + i.ToString ();
			ScreenElements.Remove (id);
		}

		for (int i = 0; i < namesCount; i ++) {
			string id = "name" + i.ToString ();
			ScreenElements.Remove (id);
			ScreenElements.AddEnabled (id, new LabelElement (playerNames[i], i+1));
		}
		
		if (showPlay) {
			ScreenElements.Enable ("play"); 
		} else {
			ScreenElements.Disable ("play");
		}

		if (namesCount >= 4) {
			ScreenElements.Disable ("invite");
		} else {
			if (MultiplayerManager.instance.Hosting && !MultiplayerManager.instance.UsingWifi) {
				ScreenElements.Enable ("invite");
			}
		}
		ScreenElements.EnableUpdating ();
	}

	protected override void OnButtonPress (ButtonPressEvent e) {
		switch (e.id) {
			case "Back": GoBack (); break;
			case "Play": PlayGame (); break;
			case "Invite More": MultiplayerManager.instance.InviteMore (); break;
		}
	}

	void GoBack () {
		Events.instance.RemoveListener<DisconnectedFromServerEvent> (OnDisconnectedFromServerEvent);
		MultiplayerManager.instance.Disconnect ();
		if (MultiplayerManager.instance.UsingWifi) {
			GoBackScreen (hosting ? "Host or Join" : "Games List");
		} else {
			GoBackScreen ("Host or Join");
		}
	}

	void PlayGame () {
		MultiplayerManager.instance.StartGame ();

		// New - Decider selection
		if (DeciderSelectionStyle.Host) {
			Player.instance.deciderManager.SetDecider (Player.instance.Name);
		}
		GameStateController.instance.AllPlayersGotoScreen ("Choose Deck", "Decider");
	}

	void OnDisconnectedFromServerEvent (DisconnectedFromServerEvent e) {
		GotoScreen ("Host or Join");
	}
}
