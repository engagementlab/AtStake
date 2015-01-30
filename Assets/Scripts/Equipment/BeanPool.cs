using UnityEngine;
using System.Collections;

public static class BeanValues {

	public static readonly int deciderStart = 5;
	public static readonly int playerStart = 3;
	public static readonly int addTime = 1;
	public static readonly int bonus1 = 1;
	public static readonly int bonus2 = 2;
	public static readonly int roundPot = 3;

}

public class BeanPool : IBeanPool {

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

	public virtual void OnRoundStart (bool isDecider=false) {
		SetBeans (isDecider ? BeanValues.deciderStart : BeanValues.playerStart);
	}

	public bool OnAddTime () {
		if (SubtractBeans (BeanValues.addTime)) {
			BeanPotManager.instance.OnAddTime ();
			return true;
		}
		return false;
	}

	public void OnAddBonus (int bonusValue) {
		AddBeans (bonusValue == 1 ? BeanValues.bonus1 : BeanValues.bonus2);
	}

	public void OnWin () {
		AddBeans (BeanPotManager.instance.OnWin ());
	}

	/**
	 *	Private functions
	 */

	public void AddBeans (int amount) {
		beanCount += amount;
		SendUpdateMessage ();
	}

	public void SetBeans (int amount) {
		beanCount = amount;
		SendUpdateMessage ();
	}

	public bool SubtractBeans (int amount) {
		if (amount > beanCount) 
			return false;
		beanCount -= amount;
		SendUpdateMessage ();
		return true;
	}

	void SendUpdateMessage () {
		Events.instance.Raise (new UpdateBeanPoolEvent (beanCount));
	}
}