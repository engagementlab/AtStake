using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ChooseDeciderScreen : GameScreen {
	
	string decider = "";
	List<ButtonElement> buttons = new List<ButtonElement> ();

	public ChooseDeciderScreen (GameState state, string name = "Choose Decider") : base (state, name) {
		ScreenElements.AddEnabled ("instructions", new LabelElement (Copy.ChooseDecider, 0));
		Events.instance.AddListener<RefreshPlayerListEvent> (OnRefreshPlayerListEvent);
	}

	public override void OnScreenStart (bool hosting, bool isDecider) {}

	void OnRefreshPlayerListEvent (RefreshPlayerListEvent e) {
		
		buttons.Clear ();
		string[] names = e.playerNames;
		for (int i = 0; i < names.Length; i ++) {
			string name = names[i];
			string id = "name" + i.ToString ();
			ScreenElements.Remove (id);
			ButtonElement button = CreateButton ("Name-Decider-" + name, i+2, name);
			ScreenElements.AddEnabled (id, button);
			button.Color = "blue";
			buttons.Add (button);
		}
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
