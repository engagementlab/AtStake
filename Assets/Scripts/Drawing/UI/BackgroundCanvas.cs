using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class BackgroundCanvas : MonoBehaviour {

	public Image topBar;
	public Image bottomBar;
	public Image background;

	void Awake () {
		SetTheme (new MultiplayerTheme ());
		Events.instance.AddListener<ChangeScreenEvent> (OnChangeScreenEvent);
	}

	public void SetTheme (BackgroundCanvasTheme theme) {
		topBar.color = theme.TopBar;
		bottomBar.color = theme.BottomBar;
		background.color = theme.Background;
	}

	void OnChangeScreenEvent (ChangeScreenEvent e) {
		string name = e.screen.name;
		BackgroundCanvasTheme theme = new DefaultTheme ();
		switch (name) {
			case "Start":
			case "About":
			case "Enter Name":
			case "Host or Join": 
			case "Games List":
			case "Choose Deck":
			case "Choose Decider": theme = new MultiplayerTheme (); break;
			case "Lobby": theme = new LobbyTheme (); break;
			case "Role": theme = new RoleTheme (); break;
			case "Bio":
			case "Agenda":
			case "Question": theme = new IntroductionTheme (); break;
			case "Brainstorm":
			case "Pitch":
			case "Deliberate":
			case "Decide": theme = new PitchTheme (); break;
			case "Agenda Item":
			case "Agenda Results":
			case "Scoreboard":
			case "Win": theme = new WinTheme (); break;
		}
		SetTheme (theme);
	}
}

public abstract class BackgroundCanvasTheme {
	public abstract Color TopBar { get; }
	public abstract Color BottomBar { get; }
	public abstract Color Background { get; }
}

public class DefaultTheme : BackgroundCanvasTheme {

	public override Color TopBar {
		get { return Palette.Grey; }
	}

	public override Color BottomBar {
		get { return Palette.White; }
	}

	public override Color Background {
		get { return Palette.Grey; }
	}
}

public class MultiplayerTheme : BackgroundCanvasTheme {

	public override Color TopBar {
		get { return Palette.Orange; }
	}

	public override Color BottomBar {
		get { return Palette.Orange; }
	}

	public override Color Background {
		get { return Palette.White; }
	}
}

public class IntroductionTheme : BackgroundCanvasTheme {

	public override Color TopBar {
		get { return Palette.Blue; }
	}

	public override Color BottomBar {
		get { return Palette.Blue; }
	}

	public override Color Background {
		get { return Palette.LtBlue; }
	}
}

public class WinTheme : BackgroundCanvasTheme {

	public override Color TopBar {
		get { return Palette.Teal; }
	}

	public override Color BottomBar {
		get { return Palette.Teal; }
	}

	public override Color Background {
		get { return Palette.LtTeal; }
	}
}

public class PitchTheme : BackgroundCanvasTheme {

	public override Color TopBar {
		get { return Palette.LtOrange; }
	}

	public override Color BottomBar {
		get { return Palette.LtPink; }
	}

	public override Color Background {
		get { return Palette.White; }
	}
}

public class DecideTheme : BackgroundCanvasTheme {

	public override Color TopBar {
		get { return Palette.LtPink; }
	}

	public override Color BottomBar {
		get { return Palette.White; }
	}

	public override Color Background {
		get { return Palette.White; }
	}
}

public class ScoreboardTheme : BackgroundCanvasTheme {

	public override Color TopBar {
		get { return Palette.Blue; }
	}

	public override Color BottomBar {
		get { return Palette.Blue; }
	}

	public override Color Background {
		get { return Palette.White; }
	}
}

public class RoleTheme : BackgroundCanvasTheme {

	public override Color TopBar {
		get { return Palette.Teal; }
	}

	public override Color BottomBar {
		get { return Palette.Teal; }
	}

	public override Color Background {
		get { return Palette.White; }
	}
}

public class LobbyTheme : BackgroundCanvasTheme {

	public override Color TopBar {
		get { return Palette.Blue; }
	}

	public override Color BottomBar {
		get { return Palette.Blue; }
	}

	public override Color Background {
		get { return Palette.LtBlue; }
	}
}