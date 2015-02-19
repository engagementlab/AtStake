using UnityEngine;
using System.Collections;

public static class Copy {

	static public readonly string About = 
		"@Stake is a game that fosters democracy, empathy, " + 
		"and creative problem solving for civic issues. " +
		"Players take on a variety of roles and pitch ideas " +
		"under a time pressure, competing to produce the best " +
		"idea in the eyes of the table's \"Decider.\"" + 
		"\n\nDesigned by the Engagement Lab at Emerson College";

	static public readonly string EnterName 			= "Type in your name";
	static public readonly string NewDeck 				= "Here's where people will build new desks";
	static public readonly string ChooseDeckHost 		= "Please choose a deck:";
	static public readonly string ChooseDeckClient 		= "Please wait while the host chooses a deck.";
	static public readonly string ChooseDecider 		= string.Format ("Please select your Decider. The Decider gets {0} coins while everyone else will get {1}.", BeanValues.deciderStart, BeanValues.playerStart);
	static public readonly string IntroBio				= "Have everyone introduce themselves, then press next.";
	static public readonly string IntroAgenda			= "Have everyone silently review their secret agenda, then press next.";
	static public readonly string QuestionInstructions	= "Decider Instructions: Give all players some time to read the question. When everyone is ready hit next.";
	static public readonly string BrainstormTimeDecider = "Time's up, press next.";
	static public readonly string BrainstormTimePlayer	= "Time's up, pencils down.";
	static public readonly string DeliberateTimeDecider = "Decider Instructions: Time’s up! Give players the chance to buy more time or press, \"I'm Done.\"";
	static public readonly string PitchTimeDecider1		= "Decider Instructions: Player 1's time is up, wait for them to press \"I'm Done\" or buy extra time";
	static public readonly string PitchTimeDecider2		= "All players' pitches are complete. Press next.";
	static public readonly string AddTime				= string.Format ("Time's up but you can buy an extra {0} seconds for {1} coins.", TimerValues.extraTime, BeanValues.addTime);
	static public readonly string AddTimeAdd			= string.Format ("+{0} Seconds", TimerValues.extraTime);
	static public readonly string AddTimeDone			= "I'm Done";
	static public readonly string DecideScreenPlayer 	= "Please wait while the Decider chooses the winning proposal";
	static public readonly string DecideScreenDecider 	= "Decider Instructions: Choose the player with the best proposal";
	static public readonly string YouWin				= "You won this round! You are the next round's Decider!";
	static public readonly string AgendaWait			= "Decider is ruling on Agendas. Feel free to weigh in.";

	static public string PlayerWin {
		get {
			return string.Format ("{0} won this round! They are the next round's Decider!", Player.instance.WinningPlayer);
		}
	}

	static public string AgendaItemVote (string playerName, string description) {
		return string.Format ("Decider Instructions: Press yes if you think {0}'s plan included this agenda item:\n{1}", playerName, description);
	}

	static public string ScoreboardPot (int coinCount) {
		return string.Format ("In the pot: {0} coins", coinCount);
	}

	static public string PitchInstructions (string playerName) {
		return string.Format ("Decider Instructions: Press play to start {0}'s pitch.", playerName);
	}

	static public string PitchTimeInstructions (string playerName) {
		return string.Format ("Decider Instructions: {0}'s time is up, wait for them to press \"I'm Done\" or buy extra time", playerName);
	}

	static public string PitchAddTime (string playerName) {
		return string.Format ("Decider Instructions: {0} bought {1} more seconds.", playerName, TimerValues.extraTime);
	}

	static public string[,] stageInstructions = new string[,] {
		{ "Brainstorm", string.Format ("Decider Instructions: Press play to start \"Think\" timer.") },
		{ "Pitch", string.Format ("Decider Instructions: Press play to start {0} pitch.", "Player 1's") },
		{ "Deliberate", string.Format ("Decider Instructions: You will now lead a conversation about the players' pitches. Press play to start the Deliberation timer.") }
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
