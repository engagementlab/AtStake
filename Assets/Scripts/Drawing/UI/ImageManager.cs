using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ImageManager : MonoBehaviour {

	public Sprite[] sprites;

	public static ImageManager instance;

	void Awake () {
		if (instance == null)
			instance = this;		
	}

	public Sprite GetSprite (string spriteName) {
		for (int i = 0; i < sprites.Length; i ++) {
			if (sprites[i].name == spriteName) {
				return sprites[i];
			}
		}
		Debug.LogError (string.Format ("No sprite named {0} exists", spriteName));
		return null;
	}
}
