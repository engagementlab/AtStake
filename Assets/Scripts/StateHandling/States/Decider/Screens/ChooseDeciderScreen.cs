using UnityEngine;
using System.Collections;

public class ChooseDeciderScreen : GameScreen {
	
	string decider = "";
	LabelElement deciderSelection;

	public ChooseDeciderScreen (GameState state, string name = "Choose Decider") : base (state, name) {
		deciderSelection = new LabelElement ("");
		SetStaticElements (new ScreenElement[] {
			new LabelElement ("The Decider gets 5 beans instead of 3. Please choose the first Decider."),
			deciderSelection
		});
		Events.instance.AddListener<RefreshPlayerListEvent> (OnRefreshPlayerListEvent);
	}

	public override void OnScreenStart (bool hosting, bool isDecider) {}

	void OnRefreshPlayerListEvent (RefreshPlayerListEvent e) {
		
		string[] names = e.playerNames;
		ScreenElement[] se = new ScreenElement[names.Length];
		for (int i = 0; i < names.Length; i ++) {
			string name = names[i];
			se[i] = CreateButton ("Name-Decider-" + name, name);
		}

		SetVariableElements (se);
	}

	protected override void OnButtonPress (ButtonPressEvent e) {
		if (e.id.Length < 13) return;
		if (e.id.Substring (0, 13) == "Name-Decider-") {
			decider = e.id.Substring (13);
			deciderSelection.Content = "You've chosen " + decider;
			DeciderSelectionManager.instance.SelectDecider (decider);
		}
	}
}
