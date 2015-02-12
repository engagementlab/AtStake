using UnityEngine;
using System.Collections;

public class ScoreboardScreen : GameScreen {
	
	public ScoreboardScreen (GameState state, string name = "Scoreboard") : base (state, name) {
		Events.instance.AddListener<UpdatedPlayerScoresEvent> (OnUpdatedPlayerScoresEvent);
		Events.instance.AddListener<MessagesMatchEvent> (OnMessagesMatchEvent);
		ScreenElements.AddEnabled ("title", new LabelElement ("Scoreboard", 0));
		ScreenElements.AddEnabled ("loading", new LabelElement ("Loading...", 1));
		ScreenElements.AddEnabled ("next", CreateBottomButton ("Next", "", "bottomPink", Side.Right));
	}

	public override void OnScreenStart (bool hosting, bool isDecider) {}

	void OnUpdatedPlayerScoresEvent (UpdatedPlayerScoresEvent e) {
		ScreenElements.SuspendUpdating ();
		ScreenElements.Disable ("loading");
		for (int i = 0; i < e.playerNames.Length; i ++) {
			string entry = string.Format ("{0}: {1} beans", e.playerNames[i], e.playerScores[i]);
			ScreenElements.Add<LabelElement> ("name" + i.ToString (), new LabelElement (entry, i+1)).Content = entry;
		}
		ScreenElements.EnableUpdating ();
	}

	protected override void OnButtonPress (ButtonPressEvent e) {
		if (e.id == "Next") {
			MessageMatcher.instance.SetMessage ("Scoreboard Next", "true");
		}
	}

	void OnMessagesMatchEvent (MessagesMatchEvent e) {
		if (e.id == "Scoreboard Next") {
			GameStateController.instance.GotoNextScreen ();
		}
	}
}
