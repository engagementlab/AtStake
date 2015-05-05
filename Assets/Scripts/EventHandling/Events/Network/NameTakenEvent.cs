using UnityEngine;
using System.Collections;

// Deprecate
public class NameTakenEvent : GameEvent {

	public readonly string name;

	public NameTakenEvent (string name) {
		this.name = name;
	}
}
