using UnityEngine;
using System.Collections;

public class Agenda : System.Object {

	public readonly AgendaItem[] items;

	public Agenda (AgendaItem[] items) {
		this.items = items;
	}
}
