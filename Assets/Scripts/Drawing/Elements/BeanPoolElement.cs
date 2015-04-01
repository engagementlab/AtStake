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

	int count = 0;
	int Count {
		get { return count; }
		set {
			count = value;
			Content = count.ToString ();
		}
	}

	float targetFontSize = 36;
	float maxFontSize = 40;
	float minFontSize = 20;

	public BeanPoolElement () {
		Events.instance.AddListener<UpdateBeanPoolEvent> (OnUpdateBeanPoolEvent);
		UpdateContent (Player.instance.MyBeanPool.BeanCount);
	}

	void UpdateContent (int beanCount) {
		float animTime = 0.5f;
		Content = beanCount.ToString ();
		CoroutineManager.Instance.IntLerp (Count, beanCount, animTime, (x) => Count = x);
		//CoroutineManager.Instance.StartCoroutine (animTime, PulseText);
	}

	void OnUpdateBeanPoolEvent (UpdateBeanPoolEvent e) {
		UpdateContent (e.beanCount);
	}

	public void SetText (Text text) {
		this.text = text;
		this.text.text = content;
	}

	void PulseText (float progress) {
		if (text == null) return;
		if (progress < 0.33f) {
			text.fontSize = (int)Mathf.Lerp (targetFontSize, maxFontSize, progress / 0.33f);
		} else if (progress < 0.67f) {
			text.fontSize = (int)Mathf.Lerp (maxFontSize, minFontSize, (progress - 0.33f) / 0.33f);
		} else {
			text.fontSize = (int)Mathf.Lerp (minFontSize, targetFontSize, (progress - 0.67f) / 0.33f);
		}
	}
}
