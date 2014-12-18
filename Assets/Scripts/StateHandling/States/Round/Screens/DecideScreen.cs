using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DecideScreen : GameScreen {
	
	LabelElement instructions;
	ButtonElement confirmButton;
	List<string> players;
	string winningPlayer = "";

	public DecideScreen (GameState state, string name = "Decide") : base (state, name) {

		Events.instance.AddListener<PlayersReceiveMessageEvent> (OnPlayersReceiveMessageEvent);

		instructions = new LabelElement ("");
		SetStaticElements (new ScreenElement[] {
			instructions
		});
	}

	protected override void OnScreenStartDecider () {
		
		RoundState round = state as RoundState;
		players = round.Players;

		instructions.content = Copy.DecideScreenDecider;
		ScreenElement[] se = new ScreenElement[players.Count+1];
		for (int i = 0; i < se.Length-1; i ++) {
			string name = players[i];
			se[i] = CreateButton ("Name-" + name, name);
		}
		confirmButton = CreateButton ("Confirm");
		se[se.Length-1] = confirmButton;

		SetVariableElements (se);
	}

	protected override void OnScreenStartPlayer () {
		instructions.content = Copy.DecideScreenPlayer;
	}

	protected override void OnButtonPress (ButtonPressEvent e) {
		
		if (e.id == "Confirm" && winningPlayer != "") {
			Player.instance.WinningPlayer = winningPlayer;
			MessageRelayer.instance.SendMessageToPlayers (winningPlayer);
			GameStateController.instance.AllPlayersGotoNextScreen ();
		}

		if (e.id.Length < 5) return;
		if (e.id.Substring (0, 5) == "Name-") {
			winningPlayer = e.id.Substring (5);
			confirmButton.Content = string.Format ("Confirm {0}", winningPlayer);
		}
	}

	void OnPlayersReceiveMessageEvent (PlayersReceiveMessageEvent e) {
		Player.instance.WinningPlayer = e.message1;
	}
}
