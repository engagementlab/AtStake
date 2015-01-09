using UnityEngine;
using System.Collections;

public class UpdateBeanPoolEvent : GameEvent {

	public readonly int beanCount;

	public UpdateBeanPoolEvent (int beanCount) {
		this.beanCount = beanCount;
	}
}
