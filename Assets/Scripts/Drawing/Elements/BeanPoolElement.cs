using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class BeanPoolElement : ScreenElement {

	string content = "";
	public string Content {
		get { return content; }
		set {
			content = value;
			text.text = content;
		}
	}
	Text text;

	public BeanPoolElement () {
		Events.instance.AddListener<UpdateBeanPoolEvent> (OnUpdateBeanPoolEvent);
	}

	void UpdateContent (int beanCount) {
		Content = string.Format ("Beans: {0}", beanCount);
	}

	void OnUpdateBeanPoolEvent (UpdateBeanPoolEvent e) {
		UpdateContent (e.beanCount);
	}

	public void SetText (Text text) {
		this.text = text;
		this.text.text = content;
	}
}
