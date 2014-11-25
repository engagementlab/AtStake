using UnityEngine;
using System.Collections;

public class Role : System.Object {

	public readonly string name;
	public readonly string bio;

	Agenda agenda;

	public Role (string name, string bio) {
		this.name = name;
		this.bio = bio;
	}

	public void SetAgendaItems (AgendaItem[] items) {
		agenda = new Agenda (items);
	}

	public void PrintAttributes () {
		Debug.Log (name);
		Debug.Log (bio);
		agenda.PrintAttributes ();
	}
}
