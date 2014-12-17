using UnityEngine;
using System.Collections;

public class BrainstormScreen : StageScreen {
	
	public BrainstormScreen (GameState state, string name = "Brainstorm") : base (state, name) {
		InitStageScreen (10f);
	}

	public override void OnCountDownEnd () {
		MessageRelayer.instance.SendMessageToPlayers ("EnableAddTime");
	}

	protected override void OnPlayersReceiveMessageEvent (PlayersReceiveMessageEvent e) {
		if (e.message1 == "EnableAddTime")
			addTimeEnabled = true;
		if (e.message1 == "DisableAddTime")
			addTimeEnabled = false;
	}
}
