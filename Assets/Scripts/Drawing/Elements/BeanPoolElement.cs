using UnityEngine;
using System.Collections;

public class BeanPoolElement : ScreenElement {

	public string content = "";

	public BeanPoolElement () {
		Events.instance.AddListener<UpdateBeanPoolEvent> (OnUpdateBeanPoolEvent);
	}

	void UpdateContent (int beanCount) {
		content = string.Format ("Beans: {0}", beanCount);
	}

	void OnUpdateBeanPoolEvent (UpdateBeanPoolEvent e) {
		UpdateContent (e.beanCount);
	}
}
