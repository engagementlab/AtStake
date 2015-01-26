using UnityEngine;
using System.Collections;

[RequireComponent (typeof (NetworkView))]
public class BeanPotManager : MonoBehaviour {

	BeanPot beanPot = new BeanPot ();

	public int BeanCount {
		get { return beanPot.BeanCount; }
	}

	static public BeanPotManager instance;

	void Awake () {
		if (instance == null)
			instance = this;
	}

	public void OnRoundStart () {
		beanPot.OnRoundStart ();
		SendSetBeanPotMessage ();
	}

	public void OnAddTime () {
		beanPot.AddBeans (BeanValues.addTime);
		SendSetBeanPotMessage ();
	}

	public int OnWin () {
		int beanCount = beanPot.BeanCount;
		beanPot.Empty ();
		return beanCount;
	}

	public void OnLose () {
		beanPot.Empty ();
	}

	void SendSetBeanPotMessage () {
		networkView.RPC ("SetBeanPot", RPCMode.All, beanPot.BeanCount);
	}

	[RPC]
	void SetBeanPot (int beanCount) {
		beanPot.SetBeanCount(beanCount);
	}
}
