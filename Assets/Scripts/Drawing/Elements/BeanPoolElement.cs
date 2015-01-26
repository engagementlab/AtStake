using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class BeanPoolElement : ScreenElement {

	string content = "";
	public string Content {
		get { return content; }
		set {
			content = value;
			if (text != null)
				text.text = content;
		}
	}
	Text text = null;

	public BeanPoolElement () {
		Events.instance.AddListener<UpdateBeanPoolEvent> (OnUpdateBeanPoolEvent);
		UpdateContent (Player.instance.MyBeanPool.BeanCount);
	}

	void UpdateContent (int beanCount) {
		Content = beanCount.ToString ();
	}

	void OnUpdateBeanPoolEvent (UpdateBeanPoolEvent e) {
		UpdateContent (e.beanCount);
	}

	public void SetText (Text text) {
		this.text = text;
		this.text.text = content;
	}
}
