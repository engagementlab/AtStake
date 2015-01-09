using UnityEngine;
using System.Collections;

public class UpdateBeanPotEvent : GameEvent {

	public readonly int beanCount;

	public UpdateBeanPotEvent (int beanCount) {
		this.beanCount = beanCount;
	}
}
