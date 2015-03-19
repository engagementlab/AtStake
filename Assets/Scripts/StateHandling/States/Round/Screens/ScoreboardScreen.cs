using UnityEngine;
using System.Collections;

public class ScoreboardScreen : GameScreen {
	
	string[] playerNames = null;
	int[] playerScores = null;

	public ScoreboardScreen (GameState state, string name = "Scoreboard") : base (state, name) {
		Events.instance.AddListener<UpdatedPlayerScoresEvent> (OnUpdatedPlayerScoresEvent);
		ScreenElements.AddEnabled ("title", new LabelElement ("Scores", 0, new HeaderTextStyle ()));
		ScreenElements.AddEnabled ("loading", new LabelElement ("Loading...", 1));
		ScreenElements.AddEnabled ("pot", new ScoreboardPotElement (6));
		ScreenElements.AddEnabled ("next", CreateNextButton ());
	}

	public override void OnScreenStart (bool hosting, bool isDecider) {
		base.OnScreenStart (hosting, isDecider);
		UpdateScreen ();
	}

	void OnUpdatedPlayerScoresEvent (UpdatedPlayerScoresEvent e) {
		playerNames = e.playerNames;
		playerScores = e.playerScores;
		UpdateScreen ();
	}

	void UpdateScreen () {
		if (playerNames == null || playerScores == null)
			return;
		ScreenElements.SuspendUpdating ();
		ScreenElements.Disable ("loading");
		for (int i = 0; i < playerNames.Length; i ++) {
			ScreenElements.Add<ScoreboardPoolElement> (
				"name" + i.ToString (), 
				new ScoreboardPoolElement (playerNames[i], playerScores[i], i+2)
			).SetContent (playerNames[i], playerScores[i]);
		}
		ScreenElements.EnableUpdating ();
	}
}
