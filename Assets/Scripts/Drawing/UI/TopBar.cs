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

	void Awake () {
		Events.instance.AddListener<ChangeScreenEvent> (OnChangeScreenEvent);
		Events.instance.AddListener<UpdateDrawerEvent> (OnUpdateDrawerEvent);
		SetPoolEnabled (false);
		SetPotEnabled (false);
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
		SetPoolEnabled (false);
		SetPotEnabled (false);
		foreach (ScreenElement element in elements) {
			if (element is BeanPoolElement) {
				SetPool (element as BeanPoolElement);
			}
			if (element is BeanPotElement) {
				SetPot (element as BeanPotElement);
			}
		}
	}

	void OnChangeScreenEvent (ChangeScreenEvent e) {
		screen = e.screen;
		elements = screen.Elements;
		UpdateScreen ();		
	}

	void OnUpdateDrawerEvent (UpdateDrawerEvent e) {
		elements = screen.Elements;
		UpdateScreen ();
	}
}
