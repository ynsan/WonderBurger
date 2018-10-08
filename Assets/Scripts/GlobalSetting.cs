using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GlobalSetting : MonoBehaviour {

	// シーン間での処理Script

	public static int Senpai_ID = 0;

	[Header("先輩の画像を挿入")]
	public characterData[] CharacterData = new characterData[3];

	void Start() {
		DontDestroyOnLoad(this.gameObject);
	}

	// タイトル画面へ戻ります
	public void GoTitle() {
		SceneManager.LoadScene ("Title_scene");
		Destroy (this.gameObject);
	}

	// もう一度
	public void Retray() {
		SceneManager.LoadScene("Title_scene");
		Destroy(this.gameObject);
		SceneManager.LoadScene("Game_scene");
	}

	[System.Serializable]
	public class characterData {

		public TextSet _senpaiTextSet_;

		[Header("先輩の画像を挿入")]
		public Sprite Senpai_images2;
		[Header("先輩のface画像を挿入")]
		public Sprite[] Senpai_images;

	}
}
