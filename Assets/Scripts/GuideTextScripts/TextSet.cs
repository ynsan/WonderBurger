using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class TextSet : MonoBehaviour {

	// is textType
	/// <summary>
	/// キャラのセリフかどうか
	/// </summary>
	
	public bool isTextSet_characterText = true;


	public IsTextSet_Group[] is_TextSetGroup;

	[System.Serializable]
	public class IsTextSet_Group {
		public string label = "";
		[Multiline]
		public string[] textSet = new string[] {
		"うまいね！新人とは思えないくらい！",
		"とっても上手だよ！",
		"センスあるね！い調子だよ！"
		};
	}
}
