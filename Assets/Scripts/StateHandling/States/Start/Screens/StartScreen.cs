using UnityEngine;
using System.Collections;

public class StartScreen : GameScreen {

	//TimerElement timer;

	public StartScreen (GameState state, string name = "Start") : base (state, name) {
		//timer = CreateTimer ("Start", 2);
		SetStaticElements (new ScreenElement[] {
			CreateButton ("Play", 0),
			//CreateButton ("Instructions"),
			//CreateButton ("Deck"),
			CreateButton ("About", 1)//,
			//timer
		});
		//timer.Interactable = false;
	}

	protected override void OnButtonPress (ButtonPressEvent e) {
		switch (e.id) {
			case "Play": GotoScreen ("Enter Name", "Multiplayer"); break;
			//case "Instructions": GotoScreen ("Instructions"); break;
			//case "Deck": GotoScreen ("New Deck"); break;
			case "About": GotoScreen ("About"); break;
			//case "Start": Timer.instance.StartCountDown (10); break;
		}
	}
}
