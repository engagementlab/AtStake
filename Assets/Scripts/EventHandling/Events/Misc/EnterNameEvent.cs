using UnityEngine;
using System.Collections;

// Deprecate
public class EnterNameEvent : GameEvent {

	public readonly string name;

	public EnterNameEvent (string name) {
		this.name = name;
	}
}
