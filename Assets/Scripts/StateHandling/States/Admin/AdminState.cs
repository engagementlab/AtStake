using UnityEngine;
using System.Collections;

public class AdminState : GameState {
	
	public AdminState (string name = "Admin") : base (name) {
		
	}
	
	public override GameScreen[] SetScreens () {
		return new GameScreen[] {
			new DecksListScreen ()
		};
	}
}
