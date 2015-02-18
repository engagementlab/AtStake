using UnityEngine;
using System.Collections;

public class ScoreboardScreen : GameScreen {
	
	LabelElement potLabel;

	public ScoreboardScreen (GameState state, string name = "Scoreboard") : base (state, name) {
		Events.instance.AddListener<UpdatedPlayerScoresEvent> (OnUpdatedPlayerScoresEvent);
		Events.instance.AddListener<UpdateBeanPotEvent> (OnUpdateBeanPotEvent);
		potLabel = new LabelElement ("Pot: 0", 1);
		ScreenElements.AddEnabled ("title", new LabelElement ("Scoreboard", 0));
		ScreenElements.AddEnabled ("pot", potLabel);
		ScreenElements.AddEnabled ("loading", new LabelElement ("Loading...", 2));
		ScreenElements.AddEnabled ("next", CreateNextButton ());
	}

	void OnUpdatedPlayerScoresEvent (UpdatedPlayerScoresEvent e) {
		ScreenElements.SuspendUpdating ();
		ScreenElements.Disable ("loading");
		for (int i = 0; i < e.playerNames.Length; i ++) {
			string entry = string.Format ("{0}: {1}", e.playerNames[i], e.playerScores[i]);
			ScreenElements.Add<LabelElement> ("name" + i.ToString (), new LabelElement (entry, i+3)).Content = entry;
		}
		ScreenElements.EnableUpdating ();
	}

	void OnUpdateBeanPotEvent (UpdateBeanPotEvent e) {
		potLabel.Content = "Pot: " + e.beanCount;
	}
}
