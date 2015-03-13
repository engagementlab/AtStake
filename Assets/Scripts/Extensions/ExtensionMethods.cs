using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public static class ExtensionMethods {

	public static Vector3 Vector3One = new Vector3 (1, 1, 1);

	public static T[] Shuffle<T> (this T[] array) {
		int n = array.Length;
		for (int i = 0; i < n; i++) {
		    int r = i + (int)(Random.value * (n - i));
		    T t = array[r];
		    array[r] = array[i];
		    array[i] = t;
		}
		return array;
	}

	public static List<T> Shuffle<T> (this List<T> array) {
		int n = array.Count;
		for (int i = 0; i < n; i++) {
		    int r = i + (int)(Random.value * (n - i));
		    T t = array[r];
		    array[r] = array[i];
		    array[i] = t;
		}
		return array;
	}

	public static void SetWidth (this RectTransform rect, float width) {
		rect.sizeDelta = new Vector2 (width, rect.sizeDelta.y);
	}

	public static void SetHeight (this RectTransform rect, float height) {
		rect.sizeDelta = new Vector2 (rect.sizeDelta.x, height);
	}

	public static void SetXPosition (this Transform transform, float x) {
		transform.position = new Vector3 (x, transform.position.y, transform.position.z);
	}

	public static void SetLocalScale (this Transform transform, float scale) {
		transform.localScale = new Vector3 (scale, scale, scale);
	}
}
