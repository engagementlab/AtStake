using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ChooseDeciderScreen : GameScreen {
	
	string decider = "";
	List<ButtonElement> buttons = new List<ButtonElement> ();

	public ChooseDeciderScreen (GameState state, string name = "Choose Decider") : base (state, name) {
		SetStaticElements (new ScreenElement[] {
			new LabelElement ("The Decider gets 5 beans instead of 3. Please choose the first Decider.", 0)
		});
		Events.instance.AddListener<RefreshPlayerListEvent> (OnRefreshPlayerListEvent);
	}

	public override void OnScreenStart (bool hosting, bool isDecider) {}

	void OnRefreshPlayerListEvent (RefreshPlayerListEvent e) {
		
		buttons.Clear ();
		string[] names = e.playerNames;
		ScreenElement[] se = new ScreenElement[names.Length];
		for (int i = 0; i < names.Length; i ++) {
			string name = names[i];
			ButtonElement button = CreateButton ("Name-Decider-" + name, i+2, name);
			buttons.Add (button);
			se[i] = button; 
		}

		SetVariableElements (se);
	}

	protected override void OnButtonPress (ButtonPressEvent e) {
		if (e.id.Length < 13) return;
		ResetButtonColors ();
		if (e.id.Substring (0, 13) == "Name-Decider-") {
			decider = e.id.Substring (13);
			DeciderSelectionManager.instance.SelectDecider (decider);
			e.element.Color = "green";
		}
	}

	void ResetButtonColors () {
		foreach (ButtonElement element in buttons) {
			element.Color = "blue";
		}
	}
}
