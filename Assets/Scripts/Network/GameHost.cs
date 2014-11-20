using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameHost : GamePlayer {

	public GameHost (string playerName) : base (playerName) {
		this.playerName = playerName;
		AddPlayer (playerName);
	}
}
