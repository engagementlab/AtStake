using UnityEngine;
using System.Collections;

public class Deck {

	string name = "";
	Role[] roles = new Role[0];

	public Role[] Roles {
		get { return roles; }
	}

	public Deck (string name, Role[] roles) {
		this.name = name;
		this.roles = roles;
	}

	public void PrintAttributes () {
		foreach (Role r in roles) {
			r.PrintAttributes ();
		}
	}
}
