using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameClient : GamePlayer {

	public GameClient (string playerName) : base (playerName) {
		this.playerName = playerName;
	}
}
