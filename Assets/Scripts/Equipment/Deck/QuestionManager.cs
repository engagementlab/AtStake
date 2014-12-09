using UnityEngine;
using System.Collections;

public class QuestionManager : MonoBehaviour {

	string[] questions;

	static public QuestionManager instance;

	void Awake () {
		if (instance == null)
			instance = this;
	}

	public void PopulateQuestions (string[] questions) {
		this.questions = questions;
	}

	public string GetQuestion (int roundNumber) {
		return questions[roundNumber];
	}
}
