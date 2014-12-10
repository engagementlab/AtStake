using UnityEngine;
using System.Collections;

public class SetRoleEvent : GameEvent {

	public readonly Role role; 

	public SetRoleEvent (Role role) {
		this.role = role;
	}
}
