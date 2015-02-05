using UnityEngine;
using System.Collections;

public class FinalScoreboardScreen : GameScreen {
	
	public FinalScoreboardScreen (GameState state, string name = "Final Scoreboard") : base (state, name) {
		Events.instance.AddListener<UpdatedPlayerScoresEvent> (OnUpdatedPlayerScoresEvent);
		ScreenElements.AddEnabled ("title", new LabelElement ("Scoreboard", 0));
		ScreenElements.AddEnabled ("menu", CreateButton ("Main Menu", 6));
	}

	public override void OnScreenStart (bool hosting, bool isDecider) {}

	void OnUpdatedPlayerScoresEvent (UpdatedPlayerScoresEvent e) {
		ScreenElements.SuspendUpdating ();
		for (int i = 0; i < e.playerNames.Length; i ++) {
			string entry = string.Format ("{0}: {1} beans", e.playerNames[i], e.playerScores[i]);
			ScreenElements.Add<LabelElement> ("name" + i.ToString (), new LabelElement (entry, i+1)).Content = entry;
		}
		ScreenElements.EnableUpdating ();
	}

	protected override void OnButtonPress (ButtonPressEvent e) {
		if (e.id == "Main Menu") {
			MultiplayerManager.instance.DisconnectGame ();
			GotoScreen ("Start", "Start");
			Events.instance.Raise (new GameEndEvent ());
			ClearScreen ();
		}
	}
}
