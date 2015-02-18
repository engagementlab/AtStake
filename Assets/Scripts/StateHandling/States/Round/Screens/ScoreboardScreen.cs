using UnityEngine;
using System.Collections;

public class ScoreboardScreen : GameScreen {
	
	LabelElement potLabel;
	string[] playerNames = null;
	int[] playerScores = null;

	public ScoreboardScreen (GameState state, string name = "Scoreboard") : base (state, name) {
		Events.instance.AddListener<UpdatedPlayerScoresEvent> (OnUpdatedPlayerScoresEvent);
		Events.instance.AddListener<UpdateBeanPotEvent> (OnUpdateBeanPotEvent);
		potLabel = new LabelElement (Copy.ScoreboardPot (0), 6);
		ScreenElements.AddEnabled ("title", new LabelElement ("Scores", 0, new HeaderTextStyle ()));
		ScreenElements.AddEnabled ("loading", new LabelElement ("Loading...", 1));
		ScreenElements.AddEnabled ("pot", potLabel);
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
			string entry = string.Format ("{0}: {1}", playerNames[i], playerScores[i]);
			ScreenElements.Add<LabelElement> ("name" + i.ToString (), new LabelElement (entry, i+2)).Content = entry;
		}
		ScreenElements.EnableUpdating ();
	}

	void OnUpdateBeanPotEvent (UpdateBeanPotEvent e) {
		potLabel.Content = Copy.ScoreboardPot (e.beanCount);
	}
}
