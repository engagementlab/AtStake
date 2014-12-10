using UnityEngine;
using System.Collections;

public class BrainstormScreen : GameScreen {
	
	TimerElement timer;
	float time = 30f;

	public BrainstormScreen (GameState state, string name = "Brainstorm") : base (state, name) {
		SetStaticElements (new ScreenElement[] {
			new LabelElement ("Brainstorm")
		});
	}

	protected override void OnScreenStartPlayer () {
		RoundState round = state as RoundState;
		SetVariableElements (new ScreenElement[] {
			new LabelElement (round.Question),
			CreateButton ("+30 Seconds"),
			CreateButton ("Role Card")
		});
	}

	protected override void OnScreenStartDecider () {

		timer = CreateTimer (time);

		RoundState round = state as RoundState;
		SetVariableElements (new ScreenElement[] {
			CreateButton ("Start Timer"),
			timer,
			new LabelElement (Copy.BrainstormInstructions),
			new LabelElement (round.Question)
		});
	}

	protected override void OnButtonPress (ButtonPressEvent e) {
		switch (e.id) {
			case "Start Timer": timer.StartCountDown (time); break;
			case "+30 Seconds": AddTime (); break;
			case "Role Card": GotoScreen ("Role"); break;
			case "Next": GameStateController.instance.AllPlayersGotoScreen ("Pitch"); break;
		}
	}

	public override void OnCountDownEnd () {
		AppendVariableElements (new ScreenElement[] {
			CreateButton ("Next")
		});
	}

	void AddTime () {
		if (Player.instance.MyBeanPool.SubtractBeans (1)) {
			Timer.instance.DeciderAddSeconds (30f);
		}
	}
}
