using UnityEngine;
using System.Collections;

public class ScreenDrawer : MonoBehaviour {

	GameState state;
	GameScreen screen;
	ScreenElement[] elements;

	void Awake () {
		Events.instance.AddListener<ChangeStateEvent> (OnChangeStateEvent);
		Events.instance.AddListener<ChangeScreenEvent> (OnChangeScreenEvent);
		Events.instance.AddListener<UpdateDrawerEvent> (OnUpdateDrawerEvent);
	}
	
	void OnGUI () {
		if (elements == null)
			return;
		foreach (ScreenElement e in elements) {
			if (e is ButtonElement) {
				ButtonElement b = e as ButtonElement;
				if (GUILayout.Button (b.content, GUILayout.Width (200), GUILayout.Height (75))) {
					Events.instance.Raise (new ButtonPressEvent (b.screen, b.id));
				}
			}
			if (e is LabelElement) {
				LabelElement l = e as LabelElement;
				GUILayout.Label (l.content);
			}
			if (e is TextFieldElement) {
				TextFieldElement t = e as TextFieldElement;
				t.content = GUILayout.TextField (t.content, 25);
			}
		}
	}

	void OnChangeStateEvent (ChangeStateEvent e) {
		state = e.state;
	}

	void OnChangeScreenEvent (ChangeScreenEvent e) {
		screen = e.screen;
		elements = screen.Elements;
	}

	void OnUpdateDrawerEvent (UpdateDrawerEvent e) {
		elements = screen.Elements;
	}
}
