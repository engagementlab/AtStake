using UnityEngine;
using System.Collections;

public class UIScreen : MonoBehaviour {

	public RectTransform canvas;
	protected float xRight;
	protected float xLeft;
	protected float scrollRight;
	protected float scrollLeft;

	public BackgroundCanvas background;
	public TopBar topBar;
	public BottomBarCanvas bottomBar;
	public MiddleCanvas middle;
	public RectTransform scrollView;

	void Awake () {
		xRight = canvas.sizeDelta.x * canvas.localScale.x;
		xLeft = -xRight;
		scrollRight = scrollView.sizeDelta.x/2;
		scrollLeft = -scrollRight;
	}

	public void SlideRight (float time) {
		if (transform.position.x == xRight) {
			transform.SetXPosition (xLeft);
			scrollView.SetXPosition (scrollLeft);
		}
		if (transform.position.x == xLeft) {
			StartCoroutine (Slide (xLeft, 0, scrollLeft, 0, time));
		} else if (transform.position.x == 0) {
			StartCoroutine (Slide (0, xRight, 0, scrollRight, time));
		}
	}

	public void SlideLeft (float time) {
		if (transform.position.x == xLeft) {
			transform.SetXPosition (xRight);
			scrollView.SetXPosition (scrollRight);
		}
		if (transform.position.x == xRight) {
			StartCoroutine (Slide (xRight, 0, scrollRight, 0, time));
		} else if (transform.position.x == 0) {
			StartCoroutine (Slide (0, xLeft, 0, scrollLeft, time));
		}
	}

	IEnumerator Slide (float start, float end, float scrollStart, float scrollEnd, float time) {

		float eTime = 0;

		while (eTime < time) {

			eTime += Time.deltaTime;
			float progress = eTime / time;

			// Background and bars
			float xPos = Mathf.Lerp (start, end, Mathf.SmoothStep (0, 1, progress));
			transform.SetXPosition (xPos);

			// Middle content
			float progress2 = 2 * progress - Mathf.Pow (progress, 2);
			float scrollPos = Map (progress2, 0, 1, scrollStart, scrollEnd);
			
			scrollView.SetXLocalPosition (scrollPos);
			yield return null;
		}

		transform.SetXPosition (end);
		scrollView.SetXLocalPosition (scrollEnd);
	}

	float Map (float s, float a1, float a2, float b1, float b2) {
		return b1 + (s-a1) * (b2-b1) / (a2-a1);
	}

	public void OnChangeScreenEvent (ChangeScreenEvent e) {
		background.OnChangeScreenEvent (e);
		bottomBar.OnChangeScreenEvent (e);
		middle.OnChangeScreenEvent (e);
	}

	public void OnUpdateDrawerEvent (UpdateDrawerEvent e) {
		bottomBar.OnUpdateDrawerEvent (e);
		middle.OnUpdateDrawerEvent (e);
	}
}
