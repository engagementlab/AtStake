using UnityEngine;
using System.Collections;

public class RoundEndManager : MonoBehaviour {

	void Awake () {
		Events.instance.AddListener<RoundEndEvent> (OnRoundEndEvent);
	}

	void OnRoundEndEvent (RoundEndEvent e) {
		
	}
}
