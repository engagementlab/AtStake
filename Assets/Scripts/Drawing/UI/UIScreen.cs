using UnityEngine;
using System.Collections;

public class UIScreen : MonoBehaviour {

	public RectTransform canvas;
	protected float xRight;
	protected float xLeft;

	public BackgroundCanvas background;
	public TopBar topBar;
	public BottomBarCanvas bottomBar;
	public MiddleCanvas middle;

	void Awake () {
		xRight = canvas.sizeDelta.x * canvas.localScale.x;
		xLeft = -xRight;
	}

	public void SlideRight (float time) {
		if (transform.position.x == xRight) {
			transform.SetXPosition (xLeft);
		}
		if (transform.position.x == xLeft) {
			StartCoroutine (Slide (xLeft, 0, time));
		} else if (transform.position.x == 0) {
			StartCoroutine (Slide (0, xRight, time));
		}
	}

	public void SlideLeft (float time) {
		if (transform.position.x == xLeft) {
			transform.SetXPosition (xRight);
		}
		if (transform.position.x == xRight) {
			StartCoroutine (Slide (xRight, 0, time));
		} else if (transform.position.x == 0) {
			StartCoroutine (Slide (0, xLeft, time));
		}
	}

	IEnumerator Slide (float start, float end, float time) {

		float eTime = 0;

		while (eTime < time) {
			eTime += Time.deltaTime;
			float xPos = Mathf.Lerp (start, end, Mathf.SmoothStep (0, 1, eTime / time));
			transform.SetXPosition (xPos);
			yield return null;
		}

		transform.SetXPosition (end);
		yield return null;
	}

	public void OnChangeScreenEvent (ChangeScreenEvent e) {
		background.OnChangeScreenEvent (e);
		topBar.OnChangeScreenEvent (e);
		bottomBar.OnChangeScreenEvent (e);
		middle.OnChangeScreenEvent (e);
	}

	public void OnUpdateDrawerEvent (UpdateDrawerEvent e) {
		topBar.OnUpdateDrawerEvent (e);
		bottomBar.OnUpdateDrawerEvent (e);
		middle.OnUpdateDrawerEvent (e);
	}
}
