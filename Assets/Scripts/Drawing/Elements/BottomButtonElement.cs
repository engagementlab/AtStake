using UnityEngine;
using System.Collections;

public enum Side { Left, Right }

public class BottomButtonElement : ButtonElement {

	Side side = Side.Left;
	public Side Side {
		get { return side; }
	}

	public BottomButtonElement (GameScreen screen, string id, string content, Side side=Side.Left) : base (screen, id, content) {
		this.side = side;
	}
}
