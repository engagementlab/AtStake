using UnityEngine;
using System.Collections;

public class FinalScoreboardScreen : GameScreen {
	
	public FinalScoreboardScreen (GameState state, string name = "Final Scoreboard") : base (state, name) {
		Events.instance.AddListener<UpdatedPlayerScoresEvent> (OnUpdatedPlayerScoresEvent);
		ScreenElements.AddEnabled ("title", new LabelElement ("Scores", 0, new HeaderTextStyle ()));
		ScreenElements.AddEnabled ("home", CreateBottomButton ("Home", "", "bottomPink", Side.Right));
	}

	public override void OnScreenStart (bool hosting, bool isDecider) {}

	void OnUpdatedPlayerScoresEvent (UpdatedPlayerScoresEvent e) {
		ScreenElements.SuspendUpdating ();
		for (int i = 0; i < e.playerNames.Length; i ++) {
			//string entry = string.Format ("{0}: {1}", e.playerNames[i], e.playerScores[i]);
			//ScreenElements.Add<LabelElement> ("name" + i.ToString (), new LabelElement (entry, i+1)).Content = entry;
			ScreenElements.Add<ScoreboardPoolElement> (
				"name" + i.ToString (), 
				new ScoreboardPoolElement (e.playerNames[i], e.playerScores[i], i+1)
			).SetContent (e.playerNames[i], e.playerScores[i]);
		}
		ScreenElements.EnableUpdating ();
	}

	protected override void OnButtonPress (ButtonPressEvent e) {
		if (e.id == "Home") {
			MultiplayerManager.instance.Disconnect ();
			GotoScreen ("Start", "Start");
			Events.instance.Raise (new GameEndEvent ());
		}
	}
}
