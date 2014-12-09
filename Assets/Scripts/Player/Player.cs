using UnityEngine;
using System.Collections;

// throw this out?

public class Player {

	string name;
	Role role;
	BeanPool beanPool;

	public Player (string name, Role role) {
		this.role = role;
		beanPool = new BeanPool (0);
	}
}
