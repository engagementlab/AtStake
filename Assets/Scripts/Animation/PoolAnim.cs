using UnityEngine;
using System.Collections;

public class PoolAnim : MonoBehaviour {

	public Transform target;
	
	IEnumerator CoMove () {
		
		float time = 1f;
		float eTime = 0f;
		Vector3 startPosition = transform.position;
	
		while (eTime < time) {
			eTime += Time.deltaTime;
			float progress = Mathf.SmoothStep (0, 1, eTime / time);
			transform.position = Vector3.Lerp (startPosition, target.position, progress);
			yield return null;
		}
	}

	void Update () {
		if (Input.GetKeyDown (KeyCode.Space)) {
			StartCoroutine (CoMove ());
		}
	}
}
