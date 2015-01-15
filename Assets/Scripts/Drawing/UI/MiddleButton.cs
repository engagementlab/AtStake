using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MiddleButton : MonoBehaviour {

	public Text text;
	public GameScreen Screen { get; private set; }
	public string ID { get; private set; }
	public string Content { get; private set; }

	public void Set (GameScreen screen, string id, string content) {
		Screen = screen;
		ID = id;
		Content = content;
		text.text = Content;
	}
}
