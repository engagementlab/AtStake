using UnityEngine;
using System.Collections;

public class AgendaWaitScreen : GameScreen {

	public AgendaWaitScreen (GameState state, string name = "Agenda Wait") : base (state, name) {
		ScreenElements.AddEnabled ("copy", new LabelElement (Copy.AgendaWait, 0, new DefaultCenterTextStyle ()));
		ScreenElements.AddEnabled ("wait", new ImageElement ("wait", 1, Palette.Grey));
	}
}
