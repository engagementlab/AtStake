using UnityEngine;
using System.Collections;

public class StartScreen : GameScreen {

	float testVal = 100f;

	public StartScreen (GameState state, string name = "Start") : base (state, name) {
		ScreenElements.AddEnabled ("play", CreateButton ("Play", 0));
		ScreenElements.AddEnabled ("about", CreateButton ("About", 1, "", "green"));
		//ScreenElements.AddEnabled ("pot", new BeanPotElement ());
		//ScreenElements.AddEnabled ("pool", new BeanPoolElement ());
		//ScreenElements.AddEnabled ("wait", CreateBottomButton ("Wait", "", "pink", Side.Right));
		//ScreenElements.Get<BottomButtonElement> ("wait").Content = "Wait";
		//ScreenElements.AddEnabled ("egl", new ImageElement ("egl_logo", 2, Palette.Grey));
		//ScreenElements.AddEnabled ("test1", CreateButton ("Test1", 2));
		//ScreenElements.AddEnabled ("test2", CreateButton ("Test2", 3));
		//ScreenElements.AddEnabled ("test3", CreateButton ("Test3", 4));
		//ScreenElements.AddEnabled ("test4", CreateButton ("Test4", 5));
		//ScreenElements.AddEnabled ("test5", CreateButton ("Test5", 6));
		//ScreenElements.AddEnabled ("test6", CreateButton ("Test6", 7));
		//ScreenElements.AddEnabled ("test7", CreateButton ("Test7", 8));
		/*ScreenElements.AddEnabled ("label231", new LabelElement ("label1", 0));
		ScreenElements.AddEnabled ("label0", new LabelElement ("label2", 1));
		ScreenElements.AddEnabled ("label1", new LabelElement ("label3", 2));
		ScreenElements.AddEnabled ("label2", new LabelElement ("label4werwerwerwerwerwerwe wer wer wer wer wer wer wer we rwe rwe", 3));
		ScreenElements.AddEnabled ("label3", new LabelElement ("label5", 4));
		ScreenElements.AddEnabled ("label4", new LabelElement ("label6", 5));
		ScreenElements.AddEnabled ("label5", new LabelElement ("label7", 6));
		ScreenElements.AddEnabled ("label6", new LabelElement ("label8werweewrrt asf asdfa ew ew e", 7));
		ScreenElements.AddEnabled ("label7", new LabelElement ("label9", 8));
		ScreenElements.AddEnabled ("label8", new LabelElement ("label10", 9));*/
		//ScreenElements.AddEnabled ("label83", new LabelElement ("label8", 10));
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
