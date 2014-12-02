using UnityEngine;
using System.Collections;

public class BeanPool : System.Object {

	int beanCount = 0;
	public int BeanCount {
		get { return beanCount; }
	}

	public BeanPool (int beanCount) {
		this.beanCount = beanCount;
	}

	public void AddBeans (int amount) {
		beanCount += amount;
	}

	public bool SubtractBeans (int amount) {
		if (amount > beanCount) 
			return false;
		beanCount -= amount;
		return true;
	}
}
