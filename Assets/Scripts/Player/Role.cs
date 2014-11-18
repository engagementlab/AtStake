using UnityEngine;
using System.Collections;

public class Role : System.Object {

	public readonly string name;
	public readonly string bio;

	Agenda agenda;

	public Role (string name, string bio) {
		this.name = name;
		this.bio = bio;
		agenda = new Agenda (
			new AgendaItem[] {
				new AgendaItem ("description", 1)
			}
		);
	}
}
