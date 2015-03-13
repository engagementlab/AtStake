using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class BeanPotElement : ScreenElement {

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

	int count = 0;
	int Count {
		get { return count; }
		set {
			count = value;
			Content = count.ToString ();
		}
	}

	public BeanPotElement () {
		Events.instance.AddListener<UpdateBeanPotEvent> (OnUpdateBeanPotEvent);
		UpdateContent (BeanPotManager.instance.BeanCount);
	}

	void UpdateContent (int beanCount) {
		//Content = beanCount.ToString ();
		CoroutineManager.Instance.IntLerp (Count, beanCount, 0.5f, (x) => Count = x);
	}

	void OnUpdateBeanPotEvent (UpdateBeanPotEvent e) {
		UpdateContent (e.beanCount);
	}

	public void SetText (Text text) {
		this.text = text;
		this.text.text = content;
	}
}
