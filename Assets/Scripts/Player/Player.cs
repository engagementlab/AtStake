using UnityEngine;
using System.Collections;

public class Player : System.Object {

	string name;
	Role role;
	BeanPool beanPool;

	public Player (string name, Role role) {
		this.role = role;
		beanPool = new BeanPool (0);
	}
}
