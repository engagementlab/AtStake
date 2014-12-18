using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public static class ExtensionMethods {

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
}
