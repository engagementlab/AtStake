using UnityEngine;
using System.Collections;

public class BeanPool {

	int beanCount = 0;
	public int BeanCount {
		get { return beanCount; }
	}
	
	public BeanPool (int beanCount) {
		this.beanCount = beanCount;
	}

	/**
	 *	Public functions
	 */

	public void OnRoundStart (bool isDecider) {
		AddBeans (isDecider ? BeanValues.deciderStart : BeanValues.playerStart);
	}

	public bool OnAddTime () {
		return SubtractBeans (BeanValues.addTime);
	}

	public void OnAddBonus (int bonusValue) {
		AddBeans (bonusValue == 1 ? BeanValues.bonus1 : BeanValues.bonus2);
	}

	/**
	 *	Private functions
	 */

	void AddBeans (int amount) {
		beanCount += amount;
		Events.instance.Raise (new UpdateBeanPoolEvent (beanCount));
	}

	bool SubtractBeans (int amount) {
		if (amount > beanCount) 
			return false;
		beanCount -= amount;
		Events.instance.Raise (new UpdateBeanPoolEvent (beanCount));
		return true;
	}
}

public static class BeanValues {

	public static readonly int deciderStart = 5;
	public static readonly int playerStart = 3;
	public static readonly int addTime = 1;
	public static readonly int bonus1 = 1;
	public static readonly int bonus2 = 2;

}