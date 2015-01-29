using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DecideScreen : GameScreen {
	
	LabelElement instructions;
	ButtonElement confirmButton;
	List<string> players;
	string winningPlayer = "";
	List<ButtonElement> buttons = new List<ButtonElement> ();

	public DecideScreen (GameState state, string name = "Decide") : base (state, name) {
		Events.instance.AddListener<AllReceiveMessageEvent> (OnAllReceiveMessageEvent);
		instructions = new LabelElement ("", 0);
		SetStaticElements (new ScreenElement[] {
			instructions
		});
	}

	protected override void OnScreenStartDecider () {
		
		RoundState round = state as RoundState;
		players = round.Players;

		buttons.Clear ();
		instructions.Content = Copy.DecideScreenDecider;
		ScreenElement[] se = new ScreenElement[players.Count+1];
		for (int i = 0; i < se.Length-1; i ++) {
			string name = players[i];
			ButtonElement button = CreateButton ("Name-" + name, i+1, name);
			buttons.Add (button);
			se[i] = button;
		}
		confirmButton = CreateButton ("Confirm", se.Length);
		se[se.Length-1] = confirmButton;

		SetVariableElements (se);
	}

	protected override void OnScreenStartPlayer () {
		instructions.Content = Copy.DecideScreenPlayer;
		SetVariableElements (new ScreenElement[] {
			new ImageElement ("wait", 1, Color.black)
		});
	}

	protected override void OnButtonPress (ButtonPressEvent e) {
		
		if (e.id == "Confirm" && winningPlayer != "") {
			MessageSender.instance.SendMessageToAll ("Winning Player", winningPlayer);
			GameStateController.instance.AllPlayersGotoNextScreen ();
		}

		if (e.id.Length < 5) return;
		if (e.id.Substring (0, 5) == "Name-") {
			ResetButtonColors ();
			e.element.Color = "green";
			winningPlayer = e.id.Substring (5);
			confirmButton.Content = string.Format ("Confirm {0}", winningPlayer);
		}
	}

	void ResetButtonColors () {
		foreach (ButtonElement element in buttons) {
			element.Color = "blue";
		}
	}

	void OnAllReceiveMessageEvent (AllReceiveMessageEvent e) {
		if (e.id == "Winning Player") {
			Player.instance.WinningPlayer = e.message1;
		}
	}
}
