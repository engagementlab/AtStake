using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {

	new string name;
	public string Name {
		get { return name; }
	}

	Role role;
	public Role MyRole {
		get { return role; }
	}

	BeanPool beanPool;
	public BeanPool MyBeanPool {
		get { return beanPool; }
	}

	static public Player instance;

	void Awake () {
		
		if (instance == null)
			instance = this;

		Events.instance.AddListener<EnterNameEvent> (OnEnterNameEvent);
		Events.instance.AddListener<SetRoleEvent> (OnSetRoleEvent);
	}

	public Player (string name, Role role) {
		this.role = role;
		beanPool = new BeanPool (0);
	}

	void OnEnterNameEvent (EnterNameEvent e) {
		name = e.name;
	}

	void OnSetRoleEvent (SetRoleEvent e) {
		role = e.role;
	}
}
