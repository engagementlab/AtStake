using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ScoreboardPotElement : ScreenElement {

	string content = "";
	public string Content {
		get { return content; }
		set { 
			content = string.Format ("In the pot: {0}", value); 
			if (text != null) {
				text.color = Palette.Grey;
				text.text = content;
			}
		}
	}

	Text text = null;

	public ScoreboardPotElement (int position) {
		Events.instance.AddListener<UpdateBeanPotEvent> (OnUpdateBeanPotEvent);
		this.Position = position;
	}

	public void SetText (Text text) {
		this.text = text;
		text.color = Palette.Grey;
		text.text = content;
	}

	void OnUpdateBeanPotEvent (UpdateBeanPotEvent e) {
		Content = e.beanCount.ToString ();
	}
}
