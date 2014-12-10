using UnityEngine;
using System.Collections;

public class DebugManager : MonoBehaviour {

	public bool enableDebug = false;
	public bool autoLoadDeck = false;
	public bool host = false;

	public string screen;
	public string state;

	void Start () {
		
		if (!enableDebug)
			return;

		Events.instance.Raise (new EnterNameEvent ("Debbuging Jenny"));
		if (host) {
			MultiplayerManager.instance.DebugCreateHost ();
		} else {
			MultiplayerManager.instance.DebugCreateClient ();
		}

		if (autoLoadDeck) {
			StartCoroutine (LoadDeck ());
		} else {
			StartCoroutine (GotoScreen ());
		}
	}

	IEnumerator LoadDeck () {
		yield return new WaitForFixedUpdate ();
		DeckManager.instance.LoadDeck ("default-deck.json", true);
		RoleManager.instance.DebugAssignRole ();
		GameStateController.instance.GotoScreen (screen, state);
	}

	IEnumerator GotoScreen () {
		yield return new WaitForFixedUpdate ();
		GameStateController.instance.GotoScreen (screen, state);
	}
}
