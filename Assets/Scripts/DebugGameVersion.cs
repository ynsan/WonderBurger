using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugGameVersion : MonoBehaviour {

	// <summary>
	// PC or IOS デバッグ
	// </summary>
	public static bool DebugMode = false;

	public bool Debug_mode = false;

	public GlobalSetting _GlobalSetting_;
	public GameController _GameController_;
	//public GameGUI_Controller _GameGUI_Controller_;

	void Awake(){
		DebugMode = Debug_mode;
	}

	void OnGUI (){
		// デバッグモードGUI
		if (Debug_mode == true)
		GUI.Label(new Rect(32, 32, 200, 54), "< DebugMode >\n");
	}

	void Start() {
		if (Debug_mode == true) {
			PlayerController.Tutorial = false;
			_GlobalSetting_ = GameObject.Find("@DontDestroyOnLoad").gameObject.GetComponent<GlobalSetting>();
			_GameController_ = GameObject.Find("@GameEventsController").gameObject.GetComponent<GameController>();
		}
	}

	void Update() {
		if (Debug_mode) {
			if (Input.GetKeyDown("1")){
				_GlobalSetting_.GoTitle ();
				Cursor.lockState = CursorLockMode.None;
				Cursor.visible = true;
			}
		}
		if (Debug_mode) {
			if (Input.GetKeyDown("2")){
				_GameController_.gameTime = 3f;
			}
		}
	}
}
