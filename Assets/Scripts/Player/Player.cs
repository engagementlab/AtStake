using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {

	new string name;
	public string Name {
		get { return name; }
	}

	//bool isDecider;
	public bool IsDecider {
		//get { return isDecider; }
		get { return deciderManager.IsDecider; }
	}

	Role role = null;
	public Role MyRole {
		get { return role; }
	}

	BeanPool beanPool = null;
	public BeanPool MyBeanPool {
		get { 
			if (beanPool == null) {
				beanPool = new BeanPool (0);
			}
			return beanPool; 
		}
	}

	string winningPlayer = "";
	public string WinningPlayer {
		get { return winningPlayer; }
		set { winningPlayer = value; }
	}

	public bool Won {
		get { return winningPlayer == name; }
	}

	static public Player instance;

	public DeciderManager deciderManager = new DeciderManager ();

	void Awake () {

		if (instance == null)
			instance = this;

		Events.instance.AddListener<EnterNameEvent> (OnEnterNameEvent);
		Events.instance.AddListener<SetRoleEvent> (OnSetRoleEvent);
		//Events.instance.AddListener<SelectDeciderEvent> (OnSelectDeciderEvent);
		Events.instance.AddListener<NameTakenEvent> (OnNameTakenEvent);
	}

	public void OnRoundStart () {
		beanPool.OnRoundStart (IsDecider);
	}

	void OnEnterNameEvent (EnterNameEvent e) {
		name = e.name;
	}

	void OnNameTakenEvent (NameTakenEvent e) {
		name = "";
	}

	void OnSetRoleEvent (SetRoleEvent e) {
		//Debug.Log ("new role: " + e.role.name);
		role = e.role;
	}

	/*void OnSelectDeciderEvent (SelectDeciderEvent e) {
		if (e.name == name) {
			isDecider = true;
		} else {
			isDecider = false;
		}
	}*/
}
