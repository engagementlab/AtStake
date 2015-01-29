using UnityEngine;
using UnityEngine.UI;
using System.Collections;

[RequireComponent (typeof (Image))]
public class StaticImage : MonoBehaviour {

	Image image;

	public Sprite Source { 
		set { image.sprite = value; }
	}

	public Color Color {
		set { image.color = value; }
	}

	void Awake () {
		image = GetComponent<Image> ();
	}
}
