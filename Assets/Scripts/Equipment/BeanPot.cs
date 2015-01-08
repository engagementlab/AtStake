using UnityEngine;
using System.Collections;

public class BeanPot : IBeanPool {

	// The BeanPot is the pool that players throw beans into
	// So every player has their own bean pool, but there's only one pot

	int beanCount = 0;
	public int BeanCount {
		get { return beanCount; }
	}

	public void OnRoundStart (bool isDecider=false) {
		beanCount = BeanValues.roundPot;
	}

	public void Empty () {
		SetBeanCount (0);
	}

	public void SetBeanCount (int count) {
		beanCount = count;
		SendUpdateMessage ();
	}

	public void AddBeans (int amount) {
		beanCount += amount;
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
		Events.instance.Raise (new UpdateBeanPotEvent (beanCount));
	}
}
