using UnityEngine;
using System.Collections;

public class StartScreen : GameScreen {

	float testVal = 100f;

	public StartScreen (GameState state, string name = "Start") : base (state, name) {
		ScreenElements.AddEnabled ("play", CreateButton ("Play", 0));
		ScreenElements.AddEnabled ("about", CreateButton ("About", 1, "", "green"));
		/*ScreenElements.AddEnabled ("test1", CreateButton ("Test1", 2));
		ScreenElements.AddEnabled ("test2", CreateButton ("Test2", 3));
		ScreenElements.AddEnabled ("test3", CreateButton ("Test3", 4));
		ScreenElements.AddEnabled ("test4", CreateButton ("Test4", 5));
		ScreenElements.AddEnabled ("test5", CreateButton ("Test5", 6));
		ScreenElements.AddEnabled ("test6", CreateButton ("Test6", 7));
		ScreenElements.AddEnabled ("test7", CreateButton ("Test7", 8));*/
	}

	protected override void OnButtonPress (ButtonPressEvent e) {
		switch (e.id) {
			case "Play": GotoScreen ("Enter Name", "Multiplayer"); break;
			case "About": GotoScreen ("About"); break;
			/*case "Test": 
				//BeanPotManager.instance.OnRoundStart (); 
				Events.instance.Raise (new UpdateBeanPotEvent (200));
				break;*/
		}
	}
}
