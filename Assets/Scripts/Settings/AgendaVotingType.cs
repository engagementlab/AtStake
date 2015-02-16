using UnityEngine;
using System.Collections;

public enum VotingType { All, Decider }

public static class AgendaVotingType {
	
	public static VotingType Type = VotingType.Decider;
	
	public static bool All {
		get { return (Type == VotingType.All); }
	}

	public static bool Decider {
		get { return (Type == VotingType.Decider); }
	}
}
