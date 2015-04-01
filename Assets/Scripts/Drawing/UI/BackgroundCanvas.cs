using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class BackgroundCanvas : MonoBehaviour {

	public Image topBar;
	public Image bottomBar;
	public Image background;
	public Image backgroundImage;

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
			case "Games List": theme = new StartTheme (); break;
			case "Scoreboard":
			case "Choose Deck":
			case "Choose Decider": 
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
		SetBackground (e.screen.Elements);
	}

	void SetBackground (ScreenElement[] elements) {
		RectTransform rectTransform = backgroundImage.rectTransform;
		bool hasBackground = false;
		foreach (ScreenElement element in elements) {
			if (element is BackgroundElement) {
				BackgroundElement b = element as BackgroundElement;
				backgroundImage.gameObject.SetActive (true);
				backgroundImage.sprite = b.Sprite;
				backgroundImage.color = b.Color;
				if (b.Anchor == "bottom") {
					rectTransform.anchoredPosition = new Vector2 (0, 125);
					rectTransform.anchorMin = new Vector2 (0.5f, 0);
					rectTransform.anchorMax = new Vector2 (0.5f, 0);
					rectTransform.pivot = new Vector3 (0.5f, 0);
				}
				if (b.Anchor == "middle") {
					rectTransform.anchoredPosition = new Vector2 (0, 0);
					rectTransform.anchorMin = new Vector3 (0.5f, 0.5f);
					rectTransform.anchorMax = new Vector3 (0.5f, 0.5f);
					rectTransform.pivot = new Vector3 (0.5f, 0.5f);
				}
				hasBackground = true;
				break;
			}
		}
		if (!hasBackground) {
			backgroundImage.gameObject.SetActive (false);
		}
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