using UnityEngine;
using System.Collections;

public class ScreenElement {
	
	public int Position {
		get; protected set;
	}

	public virtual void Draw () {}
}
