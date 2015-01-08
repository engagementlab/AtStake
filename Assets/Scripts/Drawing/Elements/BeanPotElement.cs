using UnityEngine;
using System.Collections;

public class BeanPotElement : ScreenElement {

	public string content = "Pot:";

	public BeanPotElement () {
		Events.instance.AddListener<UpdateBeanPotEvent> (OnUpdateBeanPotEvent);
	}

	void UpdateContent (int beanCount) {
		content = string.Format ("Pot: {0}", beanCount);
	}

	void OnUpdateBeanPotEvent (UpdateBeanPotEvent e) {
		UpdateContent (e.beanCount);
	}
}
