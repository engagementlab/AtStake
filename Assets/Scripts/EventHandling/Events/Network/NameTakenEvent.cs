using UnityEngine;
using System.Collections;

public class NameTakenEvent : GameEvent {

	public readonly string name;

	public NameTakenEvent (string name) {
		this.name = name;
	}
}
