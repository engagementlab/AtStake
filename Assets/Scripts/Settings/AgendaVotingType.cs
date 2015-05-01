using UnityEngine;
using System.Collections;

public enum VotingStyle { All, Decider }

public static class AgendaVotingStyle {
	
	public static VotingStyle Type = VotingStyle.Decider;
	
	public static bool All {
		get { return (Type == VotingStyle.All); }
	}

	public static bool Decider {
		get { return (Type == VotingStyle.Decider); }
	}
}
