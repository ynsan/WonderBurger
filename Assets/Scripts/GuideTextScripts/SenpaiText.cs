using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SenpaiText : MonoBehaviour {

	public bool ActiveGuideText_Update = true;

	public Text is_textBox_text;
	public Image is_textBox;
	public static string Senpai_text = "";

	public string saving_stringtext = "";

	public TextSet _TextSet_;

	private float showText_time = 0f;

	void Awake () {
		showText_time = 0f;
	}
	
	void Start() {
		// Test
		_TextSet_ = GameObject.Find("@DontDestroyOnLoad").gameObject.GetComponent<GlobalSetting>().CharacterData[GlobalSetting.Senpai_ID]._senpaiTextSet_;
	
		Senpai_text = "";
		saving_stringtext = "";
	
	}

	void Update () {

		is_textBox_text.text = Senpai_text;
		/*
		// SenpaiのText内容あり
		if (Senpai_text != "") {
			showText_time += Time.deltaTime;
			if (saving_stringtext != "" && showText_time <= 3f) {
				Senpai_text = "";
				showText_time = 0f;
			}
			// TextUpdate
			Debug.Log ("wwww");
			is_textBox_text.text = Senpai_text;
		} else {
			// TextUpdate
			is_textBox_text.text = saving_stringtext;
		}

		if (saving_stringtext != "" && ActiveGuideText_Update == true) {
			if (is_textBox.enabled != true) {
				is_textBox.enabled = true;
				is_textBox_text.enabled = is_textBox.enabled;
			}
		} else if (saving_stringtext != "" && Senpai_text != ""){
			// 何もテキストがない場合は非表示
			is_textBox.enabled = false;
			is_textBox_text.enabled = is_textBox.enabled;
		}
		*/
		if (PlayerController.PlayerDamage == true) {
				Senpai_text = "";
				Senpai_text = _TextSet_.is_TextSetGroup[1].textSet[1];
				//ObjectStatus.EnemyDestroy = false;

		}
	}
}
