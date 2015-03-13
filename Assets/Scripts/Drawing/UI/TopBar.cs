using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TopBar : MonoBehaviour {

	public Text beanPool;	
	public Text beanPot;
	public Image beanPoolImage;
	public Image beanPotImage;

	GameScreen screen;
	ScreenElement[] elements;

	Vector3 targetPoolScale;
	Vector3 targetPotScale;

	bool beansEnabled = false;

	void Awake () {
		SetPoolEnabled (false);
		SetPotEnabled (false);
		targetPoolScale = beanPoolImage.transform.localScale;
		targetPotScale = beanPotImage.transform.localScale;
	}

	void SetPool (BeanPoolElement pool) {
		pool.SetText (beanPool);
		SetPoolEnabled (true);
	}

	void SetPot (BeanPotElement pot) {
		pot.SetText (beanPot);
		SetPotEnabled (true);
	}

	void SetPoolEnabled (bool enabled) {
		beanPoolImage.enabled = enabled;
		beanPool.enabled = enabled;
	}

	void SetPotEnabled (bool enabled) {
		beanPotImage.enabled = enabled;
		beanPot.enabled = enabled;
	}

	void UpdateScreen () {
		bool enableBeans = false;
		foreach (ScreenElement element in elements) {
			if (element is BeanPoolElement) {
				SetPool (element as BeanPoolElement);
			}
			if (element is BeanPotElement) {
				SetPot (element as BeanPotElement);
				enableBeans = true;
			}
		}
		if (!enableBeans) {
			if (beansEnabled)
				Shrink ();
		} else {
			SetPoolEnabled (true);
			SetPotEnabled (true);
			if (!beansEnabled)
				Expand ();
		}
		beansEnabled = enableBeans;
	}

	public void OnChangeScreenEvent (ChangeScreenEvent e) {
		screen = e.screen;
		elements = screen.Elements;
		UpdateScreen ();		
	}

	public void OnUpdateDrawerEvent (UpdateDrawerEvent e) {
		if (elements != null) {
			elements = screen.Elements;
			UpdateScreen ();
		}
	}

	void Expand () {
		StartCoroutine (CoChangeScale (true));
	}

	void Shrink () {
		StartCoroutine (CoChangeScale (false));
	}

	IEnumerator CoChangeScale (bool expand) {
		
		float time = 0.25f;
		float eTime = 0f;

		Vector3 startPoolScale = expand ? Vector3.zero : targetPoolScale;
		Vector3 startPotScale = expand ? Vector3.zero : targetPotScale;
		Vector3 endPoolScale = expand ? targetPoolScale : Vector3.zero;
		Vector3 endPotScale = expand ? targetPotScale : Vector3.zero;

		while (eTime < time) {
			eTime += Time.deltaTime;
			float progress = Mathf.SmoothStep (0, 1, eTime / time);
			beanPoolImage.transform.localScale = Vector3.Lerp (startPoolScale, endPoolScale, progress);
			beanPool.transform.localScale = Vector3.Lerp (startPoolScale, endPoolScale, progress);
			beanPotImage.transform.localScale = Vector3.Lerp (startPotScale, endPotScale, progress);
			beanPot.transform.localScale = Vector3.Lerp (startPoolScale, endPoolScale, progress);
			yield return null;
		}

		beanPoolImage.transform.localScale = endPoolScale;
		beanPool.transform.localScale = endPoolScale;
		beanPotImage.transform.localScale = endPotScale;
		beanPot.transform.localScale = endPoolScale;

		if (!expand) {
			SetPoolEnabled (false);
			SetPotEnabled (false);
		}
	}
}
