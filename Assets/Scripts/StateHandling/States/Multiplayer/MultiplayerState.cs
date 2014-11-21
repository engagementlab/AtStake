using UnityEngine;
using System.Collections;

public class MultiplayerState : GameState {
	
	public MultiplayerState (string name = "Multiplayer") : base (name) {
		
	}
	
	public override GameScreen[] SetScreens () {
		return new GameScreen[] {
			new EnterNameScreen (),
			new HostJoinScreen (),
			new GamesListScreen (),
			new LobbyScreen ()
		};
	}
}
