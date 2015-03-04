using UnityEngine;
using System;
using System.IO;
using System.Collections;
using SimpleJSON;

[RequireComponent (typeof(NetworkView))]
public class DeckManager : MonoBehaviour {

	public Deck deck;
	public static DeckManager instance;
	DeckList deckList = new DeckList ();

	// file containing data about each deck in the directory
	string decksFilename = "_decks.json";
	
	// the loaded deck's filename & whether or not it's local
	string deckFilename = "";
	bool deckLocal = false;
	bool cakeUnlocked = false; // Easter egg

	string debugText = "";

	string LocalDecksPath {
		
		get {
			#if UNITY_WEBPLAYER
				return "http://engagementgamelab.org/atstake-mobile-decks/local/";
			#endif

			#if UNITY_STANDALONE
				return System.IO.Path.Combine (Application.streamingAssetsPath, "Decks/");
			#endif

			#if UNITY_IPHONE
				return System.IO.Path.Combine (Application.streamingAssetsPath, "Decks/");
			#endif

			#if UNITY_ANDROID
				return System.IO.Path.Combine (Application.streamingAssetsPath, "Decks/");
			#endif
		}
	}

	string HostedDecksPath {
		get { return "http://engagementgamelab.org/atstake-mobile-decks/"; }
	}

	void Start () {
		
		if (instance == null) instance = this;

		Events.instance.AddListener<HostSendMessageEvent> (OnHostSendMessageEvent);
		Events.instance.AddListener<EnterNameEvent> (OnEnterNameEvent);

		GetLocalDeckNames ();
		GetHostedDeckNames ();
	}

	void GetLocalDeckNames () {

		string path = LocalDecksPath + decksFilename;

		// the webplayer gets 'local' decks from the server anyways,
		// but it's only for testing purposes so whatev
		#if UNITY_WEBPLAYER
		StartCoroutine (WWWLoadJSON (path, false, true));
		#else
		LoadJSON (path, false);
		#endif
	}

	void GetHostedDeckNames () {
		string path = HostedDecksPath + decksFilename;
		StartCoroutine (WWWLoadJSON (path, false, false));
	}

	public void LoadDeck (string filename, bool isLocal) {
		deckFilename = filename;
		deckLocal = isLocal;
		if (isLocal) {
			GetLocalDeck (filename);
		} else {
			GetHostedDeck (filename);
		}
	}

	void GetLocalDeck (string filename) {
		string path = LocalDecksPath + filename;
		LoadJSON (path, true);
	}

	void GetHostedDeck (string filename) {
		string path = HostedDecksPath + filename;
		StartCoroutine (WWWLoadJSON (path, true, false));
	}

	public void LoadJSON (string path, bool isDeck) {
		#if UNITY_WEBPLAYER
		StartCoroutine (WWWLoadJSON (path, isDeck, true));
		#else
		ParseJSON (System.IO.File.ReadAllText (path), isDeck, true);
		#endif
	}

	IEnumerator WWWLoadJSON (string path, bool isDeck, bool isLocal) {
		WWW www = new WWW (path);
		yield return www;
		ParseJSON (www.text, isDeck, isLocal);
	}

	void ParseJSON (string content, bool isDeck, bool isLocal) {
		if (isDeck) {
			ParseDeck (content);
		} else {
			ParseDecksList (content, isLocal);
		}
	}

	void ParseDecksList (string content, bool isLocal) {
		var json = JSONNode.Parse (content);
		if (json == null) return;
		JSONArray jsonDecks = json["decks"] as JSONArray;
		if (isLocal) {
			deckList.ClearLocal ();
			for (int i = 0; i < jsonDecks.Count; i ++) {
				string name = jsonDecks[i]["name"];
				if (name == "cake" && !cakeUnlocked)
					continue;
				deckList.AddLocalDeck (name, jsonDecks[i]["filename"], jsonDecks[i]["starred"]);
			}
		} else {
			deckList.ClearHosted ();
			for (int i = 0; i < jsonDecks.Count; i ++) {
				string name = jsonDecks[i]["name"];
				if (name == "cake" && !cakeUnlocked)
					continue;
				deckList.AddHostedDeck (name, jsonDecks[i]["filename"], jsonDecks[i]["starred"]);
			}
		}
		deckList.Updated ();
	}

	void ParseDeck (string content) {
		
		var json = JSONNode.Parse (content);
		
		// Questions
		JSONArray jsonQuestions = json["questions"] as JSONArray;
		CreateQuestions (jsonQuestions);

		// Role cards
		JSONArray jsonRoles = json["roles"] as JSONArray;
		Role[] r = CreateRoles (jsonRoles);
		deck = new Deck (json["name"], r);

		OnLoadDeck ();
		
		RoleManager.instance.PopulateDeck (deck);
		AgendaItemsManager.instance.Populate (deck);
	}

	void CreateQuestions (JSONArray arr) {
		string[] qs = new string[arr.Count];
		for (int i = 0; i < qs.Length; i ++) {
			qs[i] = arr[i];
		}
		QuestionManager.instance.PopulateQuestions (qs);
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
			items[i] = new AgendaItem (Player.instance.Name, jsonItems[i]["description"], Int32.Parse(jsonItems[i]["bonus"]));
		}

		r.SetAgendaItems (items);
		return r;
	}

	void OnLoadDeck () {
		if (MultiplayerManager2.instance.Hosting) {
			MessageSender.instance.ScheduleMessage ("OnServerLoadDeck");
		}
		GameStateController.instance.GotoScreen ("Choose Decider", "Decider");
	}

	void OnHostSendMessageEvent (HostSendMessageEvent e) {
		if (e.name == "OnServerLoadDeck") {
			networkView.RPC ("OnServerLoadDeck", RPCMode.Others, deckFilename, deckLocal ? 1 : 0);
		}
	}

	void OnEnterNameEvent (EnterNameEvent e) {
		if (e.name == "#Wade") {
			cakeUnlocked = true;
			GetLocalDeckNames ();
			GetHostedDeckNames ();
		}
	}

	[RPC]
	void OnServerLoadDeck (string filename, int isLocal) {
		LoadDeck (filename, isLocal == 1);
	}

	/**
	 *	Debugging
	 */

	void OnGUI () {
		GUILayout.Label (debugText);
	}
}
