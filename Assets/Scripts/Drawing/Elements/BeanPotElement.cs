using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class BeanPotElement : ScreenElement {

	string content = "Pot:";
	public string Content {
		get { return content; }
		set {
			content = value;
			text.text = content;
		}
	}
	Text text;

	public BeanPotElement () {
		Events.instance.AddListener<UpdateBeanPotEvent> (OnUpdateBeanPotEvent);
	}

	void UpdateContent (int beanCount) {
		Content = string.Format ("Pot: {0}", beanCount);
	}

	void OnUpdateBeanPotEvent (UpdateBeanPotEvent e) {
		UpdateContent (e.beanCount);
	}

	public void SetText (Text text) {
		this.text = text;
		this.text.text = content;
	}
}
