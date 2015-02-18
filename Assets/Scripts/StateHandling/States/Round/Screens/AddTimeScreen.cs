using UnityEngine;
using System.Collections;

public class AddTimeScreen : GameScreen {

	public AddTimeScreen (GameState state, string name = "Add Time") : base (state, name) {
		ScreenElements.AddEnabled ("pool", new BeanPoolElement ());
		ScreenElements.AddEnabled ("pot", new BeanPotElement ());
		ScreenElements.AddEnabled ("instructions", new LabelElement (Copy.AddTime, 0, new WhiteTextStyle ()));
		ScreenElements.AddEnabled ("add", CreateButton ("+30 Seconds"/*Copy.AddTimeAdd*/, 1));
		ScreenElements.AddEnabled ("done", CreateButton (Copy.AddTimeDone, 2));
	}

	protected override void OnButtonPress (ButtonPressEvent e) {
		switch (e.id) {
			case "+30 Seconds": AddTime (); break;
			case "I'm Done": Done (); break;
		}
	}

	protected override void OnScreenStartPlayer () {
		ScreenElements.Get<ButtonElement> ("done").Color = "blue";
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
			GoBackScreen (currentStage);
			MessageSender.instance.SendMessageToAll ("NoAddTime");
		}
	}
}