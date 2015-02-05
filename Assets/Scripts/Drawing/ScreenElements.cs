using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ScreenElements {

	Dictionary<string, ScreenElement> elements = new Dictionary<string, ScreenElement> ();
	Dictionary<string, ScreenElement> visibleElements = new Dictionary<string, ScreenElement> ();
	GameScreen screen;

	public ScreenElement[] Elements {
		get { 
			List<ScreenElement> listElements = new List<ScreenElement> (visibleElements.Values);
			return listElements.ToArray (); 
		}
	}

	public bool Empty {
		get { return Elements.Length == 0; }
	}

	public int Count {
		get { return Elements.Length; }
	}

	bool canUpdate = true;

	public void Init (Dictionary<string, ScreenElement> elements) {
		this.elements = new Dictionary<string, ScreenElement> (elements);
		this.visibleElements = new Dictionary<string, ScreenElement> (elements);
	}

	// Adds the ScreenElement if it doesn't exist, and returns the (new or existing) ScreenElement
	public T Add<T> (string id, ScreenElement element) where T : ScreenElement {
		if (elements.ContainsKey (id)) {
			Enable (id);
			ScreenElement oldElement;
			if (elements.TryGetValue (id, out oldElement)) {
				return oldElement as T;
			}
			return null;
		} else {
			AddEnabled (id, element);
			return element as T;
		}
	}

	public void AddDisabled (string id, ScreenElement element) {
		if (!elements.ContainsKey (id)) {
			elements.Add (id, element);
		} else {
			Disable (id);
		}
	}

	public void AddEnabled (string id, ScreenElement element) {
		if (!elements.ContainsKey (id)) {
			elements.Add (id, element);
			visibleElements.Add (id, element);
		} else {
			Enable (id);
		}
	}

	public void Remove (string id) {
		elements.Remove (id);
		visibleElements.Remove (id);
	}

	public T Get<T> (string id) where T : ScreenElement {
		ScreenElement element;
		if (elements.TryGetValue (id, out element)) {
			return element as T;
		}
		return null;
	}

	public void Enable (string id) {
		if (!visibleElements.ContainsKey (id)) {
			ScreenElement element;
			if (elements.TryGetValue (id, out element)) {
				visibleElements.Add (id, element);
				RaiseUpdate ();
			}
		}
	}

	public void Disable (string id) {
		visibleElements.Remove (id);
		RaiseUpdate ();
	}

	public void DisableAll () {
		List<string> ids = new List<string>(visibleElements.Keys);
		for (int i = 0; i < ids.Count; i ++) {
			visibleElements.Remove (ids[i]);
		}
		RaiseUpdate ();
	}

	public void Clear () {
		elements.Clear ();
		visibleElements.Clear ();
	}

	public void SuspendUpdating () {
		canUpdate = false;
	}

	public void EnableUpdating () {
		canUpdate = true;
		RaiseUpdate ();
	}

	void RaiseUpdate () {
		if (canUpdate) {
			Events.instance.Raise (new UpdateDrawerEvent ());
		}
	}
}
