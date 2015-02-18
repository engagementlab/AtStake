using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class BackgroundCanvas : MonoBehaviour {

	public Image topBar;
	public Image bottomBar;
	public Image background;

	void Awake () {
		SetTheme (new StartTheme ());
	}

	public void SetTheme (BackgroundCanvasTheme theme) {
		topBar.color = theme.TopBar;
		bottomBar.color = theme.BottomBar;
		background.color = theme.Background;
	}

	public void OnChangeScreenEvent (ChangeScreenEvent e) {
		string name = e.screen.name;
		BackgroundCanvasTheme theme = new StartTheme ();
		switch (name) {
			case "Start":
			case "Enter Name":
			case "Host or Join": 
			case "Games List":
			case "Choose Decider": theme = new StartTheme (); break;
			case "Scoreboard":
			case "Choose Deck":
			case "Lobby": theme = new LobbyTheme (); break;
			case "Bio":
			case "Agenda":
			case "Question":
			case "Role": theme = new RoleTheme (); break;
			case "Brainstorm":
			case "Pitch":
			case "Deliberate":
			case "Add Time":
			case "Decide": theme = new StageTheme (); break;
			case "About":
			case "Agenda Item":
			case "Agenda Results":
			case "Win": theme = new WaitTheme (); break;
		}
		SetTheme (theme);
	}
}

public abstract class BackgroundCanvasTheme {
	public abstract Color TopBar { get; }
	public abstract Color BottomBar { get; }
	public abstract Color Background { get; }
}

public class StartTheme : BackgroundCanvasTheme {
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

public class LobbyTheme : BackgroundCanvasTheme {
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

public class WaitTheme : BackgroundCanvasTheme {
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

public class RoleTheme : BackgroundCanvasTheme {
	public override Color TopBar {
		get { return Palette.Teal; }
	}
	public override Color BottomBar {
		get { return Palette.Teal; }
	}
	public override Color Background {
		get  { return Palette.White; }
	}
}

public class StageTheme : BackgroundCanvasTheme {
	public override Color TopBar {
		get { return Palette.Orange; }
	}
	public override Color BottomBar {
		get { return Palette.Orange; }
	}
	public override Color Background {
		get { return Palette.Pink; }
	}
}