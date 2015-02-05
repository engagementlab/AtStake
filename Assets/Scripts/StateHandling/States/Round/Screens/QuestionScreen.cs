using UnityEngine;
using System.Collections;

public class QuestionScreen : GameScreen {
	
	//LabelElement title;

	public QuestionScreen (GameState state, string name = "Question") : base (state, name) {
		/*title = new LabelElement ("Round", 0);
		SetStaticElements (new ScreenElement[] {
			new BeanPoolElement (),
			new BeanPotElement (),
			title
		});	*/
		ScreenElements.AddEnabled ("pool", new BeanPoolElement ());
		ScreenElements.AddEnabled ("pot", new BeanPotElement ());
		ScreenElements.AddEnabled ("title", new LabelElement ("Round", 0));
		ScreenElements.AddEnabled ("question", new LabelElement ("", 1));
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
		} else {
			ScreenElements.Disable ("next");
		}
		ScreenElements.EnableUpdating ();
		/*title.Content = string.Format("Round {0}", roundNumber);

		ScreenElement[] se = new ScreenElement[isDecider ? 2 : 1];
		se[0] = new LabelElement (round.Question, 1);
		if (isDecider) {
			se[1] = CreateBottomButton ("Next", "", "bottomPink", Side.Right);
		}

		SetVariableElements (se);*/
	}

	protected override void OnButtonPress (ButtonPressEvent e) {
		if (e.id == "Next") {
			GameStateController.instance.AllPlayersGotoScreen ("Brainstorm");
		}
	}
}
