using UnityEngine;
using System.Collections;

public class Agenda : System.Object {

	public readonly AgendaItem[] items;

	public Agenda (AgendaItem[] items) {
		this.items = items;
	}

	public void PrintAttributes () {
		foreach (AgendaItem item in items) {
			Debug.Log (item.description);
			Debug.Log (item.bonus);
		}
	}
}
