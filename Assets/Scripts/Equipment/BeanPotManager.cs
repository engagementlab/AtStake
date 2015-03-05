using UnityEngine;
using System.Collections;

public class BeanPotManager : MonoBehaviour {

	BeanPot beanPot = new BeanPot ();

	public int BeanCount {
		get { return beanPot.BeanCount; }
	}

	static public BeanPotManager instance;

	void Awake () {
		if (instance == null)
			instance = this;

		Events.instance.AddListener<AllReceiveMessageEvent> (OnAllReceiveMessageEvent);
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
		MessageSender.instance.SendMessageToAll ("SetBeanPot", "", "", beanPot.BeanCount);
	}

	void OnAllReceiveMessageEvent (AllReceiveMessageEvent e) {
		if (e.id == "SetBeanPot") {
			SetBeanPot (e.val);
		}
	}

	void SetBeanPot (int beanCount) {
		beanPot.SetBeanCount(beanCount);
	}
}
