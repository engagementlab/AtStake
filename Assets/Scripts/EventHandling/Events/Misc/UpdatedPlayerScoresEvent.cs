using UnityEngine;
using System.Collections;

public class UpdatedPlayerScoresEvent : GameEvent {

	public readonly string[] playerNames;
	public readonly int[] playerScores;

	public UpdatedPlayerScoresEvent (string[] playerNames, int[] playerScores) {
		this.playerNames = playerNames;
		this.playerScores = playerScores;
	}
}
