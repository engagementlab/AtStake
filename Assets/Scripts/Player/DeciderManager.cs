using UnityEngine;
using System.Collections;

[System.Serializable]
public class DeciderManager : System.Object {

	[SerializeField] string deciderName = "";

	public bool IsDecider {
		get { return deciderName == Player.instance.Name; }
	}

	public DeciderManager () {
		Events.instance.AddListener<SelectDeciderEvent> (OnSelectDeciderEvent);
	}

	void OnSelectDeciderEvent (SelectDeciderEvent e) {
		deciderName = e.name;		
	}
}
