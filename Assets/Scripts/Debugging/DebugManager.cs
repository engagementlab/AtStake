using UnityEngine;
using System.Collections;

public class DebugManager : MonoBehaviour {

	public bool startOverride = false;

	public string screen;
	public string state;

	void Start () {
		if (startOverride)
			return;
		GameStateController.instance.GotoScreen (screen, state);	
	}
}
