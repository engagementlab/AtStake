using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UIManager : MonoBehaviour {

	public int topBarHeight = 200;
	public int bottomBarHeight = 125;

	public RectTransform topBar;
	public RectTransform bottomBar;
	public RectTransform bottomButtons;

	void Awake () {
		topBar.SetHeight (topBarHeight);
		bottomBar.SetHeight (bottomBarHeight);
		bottomButtons.SetHeight (bottomBarHeight);
	}
}
