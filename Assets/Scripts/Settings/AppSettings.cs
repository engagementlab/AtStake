using UnityEngine;
using System.Collections;

public class AppSettings : MonoBehaviour {

	void Awake () {
		Application.targetFrameRate = 60;
	}
}
