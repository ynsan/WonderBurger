using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

	//__________________________________________________
	//
	// ◆ UI_Sounds ◆
	//__________________________________________________

public class GUI_soundController : MonoBehaviour {

	// 対象GUI
	public GameObject _GUI;
	public bool canSelect_GUI = false;	// GUIボタン操作があるかどうか
	
	// UIの効果音
	//public AudioSource[] UI_SEs;
	private AudioSource SE_selectKey;
	private AudioSource SE_goKey;
	private AudioSource SE_backKey;
	/*
		[0]	セレクト音_特殊
		[1] 決定&進む
		[2] 戻る
		[3] 選択不可,ブザー
	 */

	void Start () {
		AudioSource[] UI_SEs = _GUI.GetComponents<AudioSource>();
		SE_selectKey = UI_SEs[0];
		SE_goKey = UI_SEs[1];
		SE_backKey = UI_SEs[2];
	}

	void Update () {
		if (canSelect_GUI == true) {
			//if (OnMouseOver()){
			if (Input.GetButtonDown ("Horizontal") || Input.GetButtonDown ("Vertical")) {
				SE_selectKey.PlayOneShot (SE_selectKey.clip);
			}
			if (Input.GetKeyDown ("enter") || Input.GetKeyDown ("spease")) {
				SE_goKey.PlayOneShot (SE_goKey.clip);
			}
			if (Input.GetKeyDown ("escape")) {
				SE_backKey.PlayOneShot (SE_backKey.clip);
			}
		}
	}
	public void OnMouseOver(){
		SE_selectKey.PlayOneShot(SE_selectKey.clip);
	}
}
