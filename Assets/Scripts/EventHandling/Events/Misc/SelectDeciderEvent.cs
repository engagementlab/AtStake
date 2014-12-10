using UnityEngine;
using System.Collections;

public class SelectDeciderEvent : GameEvent {

	public readonly string name;

	public SelectDeciderEvent (string name) {
		this.name = name;
	}
}
