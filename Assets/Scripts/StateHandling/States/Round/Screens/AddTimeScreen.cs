using UnityEngine;
using System.Collections;

public class AddTimeScreen : GameScreen {

	public AddTimeScreen (GameState state, string name = "Add Time") : base (state, name) {
		ScreenElements.AddEnabled ("instructions", new LabelElement ("Time's up but for a couple beans I can give ya 30 more seconds!!", 0));
		ScreenElements.AddEnabled ("add", CreateButton ("+30 Seconds", 1));
		ScreenElements.AddEnabled ("done", CreateButton ("I'm Done", 2));
	}

	protected override void OnButtonPress (ButtonPressEvent e) {
		switch (e.id) {
			case "+30 Seconds": AddTime (); break;
			case "I'm Done": Done (); break;
		}
	}

	void AddTime () {
		string currentStage = StageScreen.CurrentStage;
		if (Player.instance.MyBeanPool.OnAddTime ()) {
			if (currentStage == "Deliberate") {
				GameStateController.instance.AllPlayersGotoScreen (currentStage);
			} else {
				GotoScreen (currentStage);
			}
			MessageSender.instance.SendMessageToAll ("YesAddTime");
		}
	}

	void Done () {
		string currentStage = StageScreen.CurrentStage;
		if (currentStage == "Deliberate") {
			MessageMatcher.instance.SetMessage ("NoAddTime", "true");
			ScreenElements.Get<ButtonElement> ("done").Color = "green";
		} else {
			GotoScreen (currentStage);
			MessageSender.instance.SendMessageToAll ("NoAddTime");
		}
	}
}