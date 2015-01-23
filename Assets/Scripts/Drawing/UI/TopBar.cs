using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TopBar : MonoBehaviour {

	public Text beanPool;	
	public Text beanPot;

	GameScreen screen;
	ScreenElement[] elements;

	void Awake () {
		Events.instance.AddListener<ChangeScreenEvent> (OnChangeScreenEvent);
		Events.instance.AddListener<UpdateDrawerEvent> (OnUpdateDrawerEvent);
		beanPool.enabled = false;
		beanPot.enabled = false;
	}

	void SetPool (BeanPoolElement pool) {
		pool.SetText (beanPool);
		beanPool.enabled = true;
	}

	void SetPot (BeanPotElement pot) {
		pot.SetText (beanPot);
		beanPot.enabled = true;
	}

	void UpdateScreen () {
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
