using UnityEngine;
using System.Collections;

public class AgendaItem : System.Object {

	public readonly string description;
	public readonly int bonus;

	public AgendaItem (string description, int bonus) {
		this.description = description;
		this.bonus = bonus;
	}
}
