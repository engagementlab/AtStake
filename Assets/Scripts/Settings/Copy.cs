using UnityEngine;
using System.Collections;

public static class Copy {

	static public string About {
		get {
			return "@Stake is a game designed by the Engagement Lab at Emerson College";
		}
	}

	static public string Instructions {
		get {
			return "Instructions on how to play the game:\n1) purchase a new microwave and\n2) reconnect with your older sister";
		}
	}

	static public string NewDeck {
		get {
			return "Here's where people will build new desks";
		}
	}

	static public string DecideScreenPlayer {
		get {
			return "Please wait while the Decider chooses the winning proposal";
		}
	}

	static public string DecideScreenDecider {
		get {
			return "Choose the player with the best proposal";
		}
	}

	static public string[,] stageInstructions = new string[,] {
		{ "Brainstorm", string.Format ("All players brainstorm silently for {0} seconds", TimerValues.brainstorm) },
		{ "Pitch", string.Format ("Now have each player pitch their idea for {0} seconds.", TimerValues.pitch) },
		{ "Deliberate", string.Format ("Have players discuss their pitches for {0} seconds.", TimerValues.deliberate) }
	};

	static public string GetInstructions (string stageName) {
		for (int i = 0; i < stageInstructions.Length / 2; i ++) {
			string s1 = stageInstructions[i, 0];
			string s2 = stageInstructions[i, 1];
			if (s1 == stageName)
				return s2;
		}
		return "stage name not found :(";
	}
}
