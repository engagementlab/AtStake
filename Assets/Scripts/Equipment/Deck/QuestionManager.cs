using UnityEngine;
using System.Collections;

public class QuestionManager : MonoBehaviour {

	string[] questions = null;

	static public QuestionManager instance;

	void Awake () {
		if (instance == null)
			instance = this;
	}

	public void PopulateQuestions (string[] questions) {
		this.questions = questions;
	}

	public string GetQuestion (int roundNumber) {
		if (questions == null) {
			return "Error: Questions not loaded";
		}
		return questions[roundNumber];
	}
}
