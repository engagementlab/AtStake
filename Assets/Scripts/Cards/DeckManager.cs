using UnityEngine;
using System;
using System.IO;
using System.Collections;
using SimpleJSON;

public class DeckManager : MonoBehaviour {

	public Deck deck;
	string dataPath = "";
	string deckPath = "/Resources/Decks";
	public static DeckManager instance;

	string[] names = new string[0];
	string debugText = "";

	void Start () {
		if (instance == null) instance = this;
		dataPath = Application.dataPath;
		//LoadDeck ("default-deck.json");
		//GetLocalDeckNames ();
		//StartCoroutine (GetDeckPath ());
		StartCoroutine (GetHostedDecks ());
	}

	IEnumerator GetHostedDecks () {
		WWW www = new WWW ("http://engagementgamelab.org/atstake-mobile-decks/local/default-deck.json");
		yield return www;
		debugText = www.text;
	}

	IEnumerator GetDeckPath () {
		yield return null;
		/*string filePath = System.IO.Path.Combine(Application.streamingAssetsPath, "Decks/default-deck.json");
		string result = "";

		string path = "";

		#if UNITY_STANDALONE
		path = dataPath + "/StreamingAssets/Decks";
		#endif

		#if UNITY_IPHONE
		path = dataPath + "/Raw/Decks";
		#endif

		#if UNITY_ANDROID
		path = "jar:file://" + dataPath + "!/assets/Decks";
		#endif

		#if UNITY_EDITOR
		path = dataPath + "/StreamingAssets/Decks";
		#endif

		if (filePath.Contains ("://")) {
			WWW www = new WWW (filePath);
			yield return www;
			debugText = www.text;
		} else {
			debugText = System.IO.File.ReadAllText (filePath);
		}*/

	}

	public void GetLocalDeckNames () {

		string path = "";

		#if UNITY_STANDALONE
		path = dataPath + "/StreamingAssets/Decks";
		#endif

		#if UNITY_IPHONE
		path = dataPath + "/Raw/Decks";
		#endif

		#if UNITY_ANDROID
		path = "jar:file://" + dataPath + "!/assets/Decks";
		#endif

		#if UNITY_EDITOR
		path = dataPath + "/StreamingAssets/Decks";
		#endif

		/*DirectoryInfo dir = new DirectoryInfo (path);

		FileInfo[] info = dir.GetFiles ("*.json");
		names = new string[info.Length];
		for (int i = 0; i < info.Length; i ++) {
			names[i] = info[i].Name;
		}*/

		// this will not work for android
		// http://answers.unity3d.com/questions/569445/does-directoryinfo-getfiles-work-on-android-.html
		/*DirectoryInfo dir = new DirectoryInfo (dataPath + deckPath);
		debugText += dataPath + deckPath;

		FileInfo[] info = dir.GetFiles ("*.json");
		names = new string[info.Length];
		for (int i = 0; i < info.Length; i ++) {
			names[i] = info[i].Name;
		}*/
	}

	public void LoadDeck (string deckName) {
		string path = dataPath + deckPath + "/" + deckName;
		#if UNITY_WEBPLAYER
		StartCoroutine (WWWLoadDeck (path));
		#else
		ParseJSON (System.IO.File.ReadAllText (path));
		#endif
	}

	IEnumerator WWWLoadDeck (string path) {
		WWW www = new WWW ("file:///" + path);
		yield return www;
		ParseJSON (www.text);
	}

	void ParseJSON (string content) {
		var json = JSONNode.Parse (content);
		JSONArray jsonRoles = json["roles"] as JSONArray;
		Role[] r = CreateRoles (jsonRoles);
		deck = new Deck (json["name"], r);
		Events.instance.Raise (new LoadDeckEvent ());
	}

	Role[] CreateRoles (JSONArray jsonRoles) {
		Role[] r = new Role[jsonRoles.Count];
		for (int i = 0; i < jsonRoles.Count; i ++) {
			r[i] = CreateRole (jsonRoles[i]);
		}
		return r;
	}

	Role CreateRole (JSONNode node) {
		
		Role r = new Role (node["name"], node["bio"]);
		
		JSONArray jsonItems = node["agenda_items"] as JSONArray;
		AgendaItem[] items = new AgendaItem[jsonItems.Count]; 
		
		for (int i = 0; i < jsonItems.Count; i ++) {
			items[i] = new AgendaItem (jsonItems[i]["description"], Int32.Parse(jsonItems[i]["bonus"]));
		}

		r.SetAgendaItems (items);
		return r;
	}

	void OnGUI () {
		for (int i = 0; i < names.Length; i ++) {
			GUILayout.Label (names[i]);
		}
		GUILayout.Label (debugText);
	}
}
