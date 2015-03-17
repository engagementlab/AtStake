using UnityEngine;
using System.Collections;

public delegate void OnWaitForFrame ();

public class CoroutineManager : MonoBehaviour {

	static CoroutineManager instanceInternal = null;
	
	static public CoroutineManager Instance {
		get {
			if (instanceInternal == null) {
				instanceInternal = Object.FindObjectOfType (typeof (CoroutineManager)) as CoroutineManager;
				if (instanceInternal == null) {
					GameObject go = new GameObject ("CoroutineManager");
					DontDestroyOnLoad (go);
					instanceInternal = go.AddComponent<CoroutineManager>();
				}
			}
			return instanceInternal;
		}
	}

	public void FloatLerp (float from, float to, float time, System.Action<float> action) {
		StartCoroutine (CoFloatLerp (from, to, time, action));
	}

	public IEnumerator CoFloatLerp (float from, float to, float time, System.Action<float> action) {
		
		float eTime = 0f;

		while (eTime < time) {
			eTime += Time.deltaTime;
			float progress = eTime / time;
			action (Mathf.Lerp (from, to, Mathf.SmoothStep (0, 1, progress)));
			yield return null;
		}

		action (to);
	}

	public void IntLerp (int from, int to, float time, System.Action<int> action) {
		StartCoroutine (CoIntLerp (from, to, time, action));
	}

	public IEnumerator CoIntLerp (int from, int to, float time, System.Action<int> action) {
		
		float eTime = 0f;

		while (eTime < time) {
			eTime += Time.deltaTime;
			float progress = eTime / time;
			action ((int)Mathf.Lerp (from, to, Mathf.SmoothStep (0, 1, progress)));
			yield return null;
		}

		action (to);
	}

	public void WaitForFrame (OnWaitForFrame onWaitForFrame) {
		StartCoroutine (CoWaitForFixedUpdate (onWaitForFrame));
	}

	IEnumerator CoWaitForFixedUpdate (OnWaitForFrame onWaitForFrame) {
		yield return new WaitForFixedUpdate ();
		yield return new WaitForFixedUpdate ();
		onWaitForFrame ();
	}
}
