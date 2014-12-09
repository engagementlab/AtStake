using UnityEngine;
using System.Collections;

public class ChooseDeciderScreen : GameScreen {
	
	string decider = "";
	LabelElement deciderSelection;

	public ChooseDeciderScreen (string name = "Choose Decider") : base (name) {
		deciderSelection = new LabelElement ("");
		SetStaticElements (new ScreenElement[] {
			new LabelElement ("Choose the first decider"),
			deciderSelection
		});
		Events.instance.AddListener<RefreshPlayerListEvent> (OnRefreshPlayerListEvent);
	}

	void OnRefreshPlayerListEvent (RefreshPlayerListEvent e) {
		
		string[] names = e.playerNames;
		ScreenElement[] se = new ScreenElement[names.Length];
		for (int i = 0; i < names.Length; i ++) {
			string name = names[i];
			se[i] = CreateButton ("Name-Decider-" + name, name);
		}

		SetVariableElements (se);
	}

	public override void OnButtonPress (ButtonPressEvent e) {
		if (e.id.Length < 13) return;
		if (e.id.Substring (0, 13) == "Name-Decider-") {
			decider = e.id.Substring (13);
			deciderSelection.content = "You've chosen " + decider;
			DeciderSelectionManager.instance.SelectDecider (decider);
		}
	}
}
