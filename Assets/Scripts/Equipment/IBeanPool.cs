using UnityEngine;
using System.Collections;

public interface IBeanPool {

	int BeanCount { get; }

	void AddBeans (int amount);
	bool SubtractBeans (int amount);
}
