using UnityEngine;
using System.Collections;

public class QuestionScreen : GameScreen {
	
	public QuestionScreen (GameState state, string name = "Question") : base (state, name) {
		ScreenElements.AddEnabled ("pool", new BeanPoolElement ());
		ScreenElements.AddEnabled ("pot", new BeanPotElement ());
		ScreenElements.AddEnabled ("title", new LabelElement ("Round", 0));
		ScreenElements.AddEnabled ("question", new LabelElement ("", 1));
		ScreenElements.AddDisabled ("instructions", new LabelElement (Copy.QuestionInstructions, 2, new DeciderInstructionsStyle ()));
		ScreenElements.AddDisabled ("next", CreateBottomButton ("Next", "", "bottomPink", Side.Right));
	}

	public override void OnScreenStart (bool hosting, bool isDecider) {
		
		ScreenElements.SuspendUpdating ();
		RoundState round = state as RoundState;
		int roundNumber = round.RoundNumber;
		ScreenElements.Get<LabelElement> ("title").Content = string.Format("Round {0}", roundNumber);
		ScreenElements.Get<LabelElement> ("question").Content = round.Question;
		if (isDecider) {
			ScreenElements.Enable ("next");
			ScreenElements.Enable ("instructions");
		} else {
			ScreenElements.Disable ("next");
			ScreenElements.Disable ("instructions");
		}
		ScreenElements.EnableUpdating ();
	}

	protected override void OnButtonPress (ButtonPressEvent e) {
		if (e.id == "Next") {
			GameStateController.instance.AllPlayersGotoScreen ("Brainstorm");
		}
	}
}
