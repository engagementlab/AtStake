using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DecideScreen : GameScreen {
	
	List<string> players;
	string winningPlayer = "";
	List<ButtonElement> buttons = new List<ButtonElement> ();

	public DecideScreen (GameState state, string name = "Decide") : base (state, name) {
		Events.instance.AddListener<AllReceiveMessageEvent> (OnAllReceiveMessageEvent);
		ScreenElements.AddEnabled ("background", new BackgroundElement ("hourglass", Color.white, "middle"));
		ScreenElements.AddDisabled ("instructionsDecider", new LabelElement (Copy.DecideScreenDecider, 0, new WhiteTextStyle ()));
		ScreenElements.AddDisabled ("instructionsPlayer", new LabelElement (Copy.DecideScreenPlayer, 0, new WhiteTextStyle ()));
		ScreenElements.AddDisabled ("confirm", CreateBottomButton ("Next", "", "bottomPink", Side.Right));
	}

	protected override void OnScreenStartDecider () {
		
		RoundState round = state as RoundState;
		players = round.Players;

		buttons.Clear ();
		ScreenElements.Enable ("instructionsDecider");
		ScreenElements.Disable ("background");

		for (int i = 0; i < players.Count; i ++) {
			string name = players[i];
			ButtonElement button = ScreenElements.Add<ButtonElement> ("name" + i.ToString (), CreateButton ("Name-" + name, i+1, name));
			button.Color = "blue";
			buttons.Add (button);
		}
	}

	protected override void OnScreenStartPlayer () {
		ScreenElements.DisableAll ();
		ScreenElements.Enable ("background");
		ScreenElements.Enable ("instructionsPlayer");
	}

	protected override void OnButtonPress (ButtonPressEvent e) {
		
		if (e.id == "Next" && winningPlayer != "") {
			MessageSender.instance.SendMessageToAll ("Winning Player", winningPlayer);
			GameStateController.instance.AllPlayersGotoNextScreen ();
		}

		if (e.id.Length < 5) return;
		if (e.id.Substring (0, 5) == "Name-") {
			ResetButtonColors ();
			e.element.Color = "green";
			winningPlayer = e.id.Substring (5);
			ScreenElements.Enable ("confirm");
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
