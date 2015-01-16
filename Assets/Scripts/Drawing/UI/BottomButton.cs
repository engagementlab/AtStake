using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class BottomButton : MonoBehaviour {

	public Image image;
	public Text text;
	public GameScreen Screen { get; private set; }
	public string ID { get; private set; }
	public string Content { get; private set; }

	public void Set (GameScreen screen, string id, string content, Color color) {
		SetEnabled (true);
		Screen = screen;
		ID = id;
		Content = content;
		text.text = Content;
		image.color = color;
	}

	public void SetEnabled (bool enabled) {
		image.enabled = enabled;
		text.enabled = enabled;
	}
}

