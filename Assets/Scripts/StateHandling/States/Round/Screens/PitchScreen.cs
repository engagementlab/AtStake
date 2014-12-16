using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PitchScreen : StageScreen {
	
	List<string> players;
	int currentPlayer = 0;
	LabelElement pitcher;

	public PitchScreen (GameState state, string name = "Pitch") : base (state, name) {
		pitcher = new LabelElement ("");
		InitStageScreen (10f);
	}

	protected override void OnScreenStartDecider () {
		InitDeciderScreen ();

		players = MultiplayerManager.instance.Players;
		players.Remove (Player.instance.Name);
		
		UpdatePitcher ();
		AppendVariableElements (new ScreenElement[] {
			pitcher
		});
	}

	protected override void OnScreenStartPlayer () {
		InitPlayerScreen ();
		AppendVariableElements (new ScreenElement[] {
			pitcher
		});
	}

	protected override void OnPlayerSendMessageEvent (PlayerSendMessageEvent e) {
		pitcher.content = "your turn";
	}

	protected override void OnOthersSendMessageEvent (OthersSendMessageEvent e) {
		pitcher.content = "";
	}

	protected override bool StartTimer () {
		
		if (currentPlayer >= players.Count-1)
			return false;

		currentPlayer ++;
		UpdatePitcher ();
		return true;
	}

	void UpdatePitcher () {
		string currPlayerName = players[currentPlayer];
		pitcher.content = currPlayerName + "'s turn";
		Events.instance.Raise (new SendMessageToPlayerEvent (currPlayerName));
		Events.instance.Raise (new SendMessageToOthersEvent (currPlayerName));
	}

	/*TimerElement timer;
	float time = 10f;
	List<string> players;
	int currentPlayer = 0;
	LabelElement pitcher;

	public PitchScreen (GameState state, string name = "Pitch") : base (state, name) {
		Events.instance.AddListener<PlayerSendMessageEvent> (OnPlayerSendMessageEvent);
		Events.instance.AddListener<OthersSendMessageEvent> (OnOthersSendMessageEvent);
		RoundState round = state as RoundState;
		SetStaticElements (new ScreenElement[] {
			new LabelElement ("Pitch"),
			new LabelElement (round.Question)
		});
	}

	protected override void OnScreenStartPlayer () {
		SetVariableElements (new ScreenElement[] {
			CreateButton ("Role Card")
		});
	}

	protected override void OnScreenStartDecider () {
		
		// Uncomment to randomize players
		//players = RandomizedPlayers (Player.instance.Name);
		players = MultiplayerManager.instance.Players;
		players.Remove (Player.instance.Name);
		
		LabelElement title = new LabelElement (Copy.PitchInstructions);
		pitcher = PitcherLabel ();
		ButtonElement timerButton = CreateButton ("Start Timer");
		timer = CreateTimer (time);
		ButtonElement nextButton = CreateButton ("Next");

		SetVariableElements (new ScreenElement[] {
			title,
			pitcher,
			timerButton,
			timer,
			nextButton
		});
	}

	List<string> RandomizedPlayers (string deciderName) {
		List<string> players = MultiplayerManager.instance.Players;
		players.Remove (deciderName);
		return players.Shuffle ();
	}

	protected override void OnButtonPress (ButtonPressEvent e) {
		switch (e.id) {
			case "Start Timer": timer.StartCountDown (); break;
			case "Next": GameStateController.instance.AllPlayersGotoScreen ("Deliberate"); break;
			case "Role Card": GotoScreen ("Role"); break;
		}
	}

	public override void OnCountDownEnd () {
		currentPlayer ++;
		pitcher = PitcherLabel ();
		Events.instance.Raise (new SendMessageToPlayerEvent (players[currentPlayer]));
	}

	LabelElement PitcherLabel () {
		return new LabelElement (players[currentPlayer] + "'s turn");
	}

	void OnPlayerSendMessageEvent (PlayerSendMessageEvent e) {
		// the drawer should handle this
		SetVariableElements (new ScreenElement[] {
			new LabelElement ("Your turn!"),
			CreateButton ("+30 seconds"),
			CreateButton ("Role Card")
		});
	}

	void OnOthersSendMessageEvent (OthersSendMessageEvent e) {
		// the drawer should handle this
		SetVariableElements (new ScreenElement[] {
			CreateButton ("Role Card")
		});	
	}*/
}
