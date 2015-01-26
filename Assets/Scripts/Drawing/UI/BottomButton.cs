using UnityEngine;
using System.Collections;
using UnityEngine.UI;

// TODO: This should inherit from MiddleButton
// Also, every parameter is contained within ButtonElement so it's redundant to pass in screen, id, content, etc.
/*public class BottomButton : MonoBehaviour {

	public Image image;
	public Text text;
	public GameScreen Screen { get; private set; }
	public string ID { get; private set; }
	public string Content { get; private set; }
	public ButtonElement Element { get; private set; }

	public void Set (GameScreen screen, string id, string content, Color color, ButtonElement element) {
		SetEnabled (true);
		Screen = screen;
		ID = id;
		Content = content;
		text.text = Content;
		image.color = color;
		Element = element;
	}

	public void SetEnabled (bool enabled) {
		image.enabled = enabled;
		text.enabled = enabled;
	}
}*/

public class BottomButton : MiddleButton {

}