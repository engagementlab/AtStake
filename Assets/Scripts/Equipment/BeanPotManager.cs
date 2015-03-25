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
		Events.instance.AddListener<HostSendMessageEvent> (OnHostSendMessageEvent);
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
		MessageSender.instance.ScheduleMessage (new NetworkMessage ("SetBeanPot", "", "", beanPot.BeanCount));
	}

	void OnAllReceiveMessageEvent (AllReceiveMessageEvent e) {
		if (e.id == "SetBeanPot") {
			SetBeanPot (e.val);
		}
	}

	void OnHostSendMessageEvent (HostSendMessageEvent e) {
		if (e.name == "SetBeanPot") {
			MessageSender.instance.SendMessageToAll (e.name, "", "", e.val);
		}
	}

	void SetBeanPot (int beanCount) {
		beanPot.SetBeanCount(beanCount);
	}
}
