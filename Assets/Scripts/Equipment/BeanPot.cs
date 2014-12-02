using UnityEngine;
using System.Collections;

public class BeanPot : BeanPool {

	// The BeanPot is the pool that players throw beans into
	// So every player has their own bean pool, but there's only one bean pot

	public BeanPot (int beanCount) : base (beanCount) {

	}
}
