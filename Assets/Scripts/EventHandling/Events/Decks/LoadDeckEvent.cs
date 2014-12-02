using UnityEngine;
using System.Collections;

public class LoadDeckEvent : GameEvent {

	public string filename;

	public LoadDeckEvent (string filename) {
		this.filename = filename;
	}
}
