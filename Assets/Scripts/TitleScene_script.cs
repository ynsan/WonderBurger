using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

// タイトル画面を制御するScript

public class TitleScene_script : MonoBehaviour {

	private int SenpaiID = 0;
	private int SubmitSenpaiID = 0;

	// SetActiveを使用するため, GameObjectで子Canvasをまとめます
	public GameObject[] CanvasPanels;
	// Buttonの初期セレクトButtonをアタッチ
	public Button[] firstSetButtons_Selectable;

	private int isCanvas_int = 0;
	//private int goCanvas_int = 0;
	private int backCanvas_int = 0;

	// ギャラリー時の選択キャラID
	private int gallary_chrID = 0;

	// 重なり順
	public Transform[] menuButtons_layer;

	//public Button selectSenpai_btn;
	public Image SenpaiBody_Image;
	public Image SenpaiInfo_Image;
	[Header("先輩のGUIデータ")]
	public Button[] SenpaiSetButtons_Selectable = new Button[3];
	public senpaiGUI_data[] senpaiGUI_Data = new senpaiGUI_data[3];
	public GameObject SenpaiSelectButton_parent;

	[Header("Do you Tutorial Game?")]
	public GameObject dytg_canvas;  // 確実性重視、他Canvasと分ける
	public Button dytg_setSelect_botton;

	[Header("Gallary_GUI")]
	public Image Gal_SenpaiChr_image;
	public Image Gal_SenpaiInfo_image;
	public Image Gal_SenpaiSIbutton_image;
	public Image Gal_SenpaiBody_image;

	private float canvasTL = 0;

	// Audio
	public Transform BGM_obj;
	private AudioSource[] BGM_audio = new AudioSource[2];

	//public bool nectCanvasmovedAwake = true;


	void Awake() {
		Time.timeScale = 1;
		//nectCanvasmovedAwake = true;
	}

	void Start() {

		// 初期表示処理
		for (int i = 1; i < CanvasPanels.Length; i++) {
			CanvasPanels[i].SetActive(false);
		}
		dytg_canvas.SetActive(false);

		canvasTL = 0;

		isCanvas_int = 0;
		SenpaiID = 0;
		SubmitSenpaiID = 0;

		senpaiImage_preview_bool = false;

		SenpaiSubmit();

		// Audio
		AudioSource[] audioSources = BGM_obj.GetComponents<AudioSource>();
		BGM_audio[0] = audioSources[0];
		BGM_audio[1] = audioSources[1];

		BGM_audio[0].Play();
	}

	void Update() {

		// Canvas変更ウェイト
		// 連打Canvas切り替え防止
		canvasTL += Time.deltaTime;
		if (canvasTL >= 1.5f) {
			// タップスタート
			if (isCanvas_int == 0 && (Input.GetMouseButtonDown(0) || Input.anyKeyDown)) {
				CanvasPanels[0].SetActive(false);
				CanvasPanels[1].SetActive(true);
				// タイトルメニューの移行先int
				isCanvas_int = 1;
				backCanvas_int = 0;
				CanvasPanels[2].SetActive(false);
				BGM_audio[0].Stop();
				BGM_audio[1].Play();

				firstSetButtons_Selectable[0].Select();
				canvasTL = 0;
			}
		}
		if (canvasTL >= 0.8f && senpaiImage_preview_bool == true) {
			// ギャラリー時のボタン操作
			if (senpaiImage_preview_bool == true && (Input.GetMouseButtonDown(0) || Input.GetButtonDown("Fire4"))) {
				CloseSenpai_sImg();
			}
		}

		Debug.Log(backCanvas_int);

		// シーンCanvas処理の開始ウェイト
		//if (nectCanvasmovedAwake == true) {

		//}
	}

	public void MenuButton_Start() {
		if (canvasTL >= 0.1f) {
			CanvasPanels[isCanvas_int].SetActive(false);
			isCanvas_int = 2;
			CanvasPanels[isCanvas_int].SetActive(true);
			backCanvas_int = 1;
			firstSetButtons_Selectable[1].Select();
			canvasTL = 0;
		}
	}

	public void MenuButton_Gallary() {
		if (canvasTL >= 0.1f) {
			CanvasPanels[isCanvas_int].SetActive(false);
			isCanvas_int = 3;
			CanvasPanels[isCanvas_int].SetActive(true);
			backCanvas_int = 1;
			firstSetButtons_Selectable[2].Select();
			canvasTL = 0;
		}
	}

	public void MenuButton_Copylight() {
		if (canvasTL >= 0.1f) {
			CanvasPanels[isCanvas_int].SetActive(false);
			isCanvas_int = 4;
			CanvasPanels[isCanvas_int].SetActive(true);
			backCanvas_int = 1;
			firstSetButtons_Selectable[3].Select();
			canvasTL = 0;
		}
	}

	public void Button_Back() {

		CanvasPanels[backCanvas_int].SetActive(true);
		CanvasPanels[isCanvas_int].SetActive(false);
		if (isCanvas_int == 5 || isCanvas_int == 6) {
			CanvasPanels[3].gameObject.SetActive(false);
			CanvasPanels[1].gameObject.SetActive(true);
			isCanvas_int = 1;
			backCanvas_int = 0;
		} else {
			isCanvas_int = backCanvas_int;
		}
		// タイトルトップへ戻る
		if (isCanvas_int == 0) {
			BGM_audio[1].Stop();
			BGM_audio[0].Play();
			// 選択を解除
			Animator _animator = CanvasPanels[isCanvas_int].GetComponent<Animator>();
			var info = _animator.GetCurrentAnimatorStateInfo(0);
			_animator.Play(info.nameHash, 0, 0.0f);
		} else if (isCanvas_int == 1) {
			backCanvas_int = 0;
		}

		firstSetButtons_Selectable[backCanvas_int].Select();
	}

	public void Button_TutorialQ_on() {
		dytg_canvas.SetActive(true);
		dytg_setSelect_botton.Select();
	}
	public void Button_TutorialQ_off() {
		dytg_canvas.SetActive(false);
		SenpaiSetButtons_Selectable[SenpaiID].Select();
		SenpaiSelect();
	}
	public void Button_TutorialOn() {
		PlayerController.Tutorial = true;
		Button_GameStart();
	}
	public void Button_TutorialOff() {
		PlayerController.Tutorial = false;
		Button_GameStart();
	}

	private void Button_GameStart() {
		// ゲームSceneに移動
		SceneManager.LoadScene("Game_scene");
		GlobalSetting.Senpai_ID = SubmitSenpaiID;
	}

	// 先輩Button
	public void Button_SenpaiChara1() {
		SenpaiID = 0;
		SenpaiSelect();
	}
	public void Button_SenpaiChara2() {
		SenpaiID = 1;
		SenpaiSelect();
	}
	public void Button_SenpaiChara3() {
		SenpaiID = 2;
		SenpaiSelect();
	}

	public void SenpaiSubmit() {
		//SenpaiSetButtons_Selectable [SenpaiID].image.sprite =  senpaiGUI_Data[SubmitSenpaiID].SenpaiSelectButton_img;
		SubmitSenpaiID = SenpaiID;
		//SenpaiSetButtons_Selectable [SubmitSenpaiID].image.sprite =  senpaiGUI_Data[SubmitSenpaiID].SenpaiDeselectButton_img;
		OKSelect();
	}
	public void SenpaiSelect() {
		SenpaiBody_Image.sprite = senpaiGUI_Data[SenpaiID].SenpaiHero_img;
		SenpaiInfo_Image.sprite = senpaiGUI_Data[SenpaiID].Senpaipro_img;
	}

	public void OKSelect() {
		SenpaiBody_Image.sprite = senpaiGUI_Data[SubmitSenpaiID].SenpaiHero_img;
		SenpaiInfo_Image.sprite = senpaiGUI_Data[SubmitSenpaiID].Senpaipro_img;
	}

	public void GallarySenpaiSelect_Button1() {
		gallary_chrID = 0;
		GallarySenpaiCanvas_StartUp();
	}
	public void GallarySenpaiSelect_Button2() {
		gallary_chrID = 1;
		GallarySenpaiCanvas_StartUp();
	}
	public void GallarySenpaiSelect_Button3() {
		gallary_chrID = 2;
		GallarySenpaiCanvas_StartUp();
	}

	private void GallarySenpaiCanvas_StartUp() {
		Gal_SenpaiChr_image.sprite = senpaiGUI_Data[gallary_chrID].Senpai_img;
		Gal_SenpaiInfo_image.sprite = senpaiGUI_Data[gallary_chrID].GalSenpaiInfo_img;
		Gal_SenpaiSIbutton_image.sprite = senpaiGUI_Data[gallary_chrID].GalButton_img;
		isCanvas_int = 5;
		backCanvas_int = 3;
		CanvasPanels[isCanvas_int].gameObject.SetActive(true);
		//CanvasPanels[3].gameObject.SetActive(false);
	}

	private bool senpaiImage_preview_bool = false;

	public void SenpaiSpecialImage_On() {
		canvasTL = 0;
		senpaiImage_preview_bool = true;
		CanvasPanels[6].GetComponent<Image>().sprite = senpaiGUI_Data[gallary_chrID].GallrySenpai_img;
		CanvasPanels[6].gameObject.SetActive(true);
	}
	public void SenpaiBodyImage_On(){
		canvasTL = 0;
		senpaiImage_preview_bool = true;
		Gal_SenpaiBody_image.sprite = senpaiGUI_Data[gallary_chrID].Senpai_img;
		CanvasPanels[7].gameObject.SetActive(true);
	}

	private void CloseSenpai_sImg () {
		senpaiImage_preview_bool = false;
		CanvasPanels[6].gameObject.SetActive(false);
		CanvasPanels[7].gameObject.SetActive(false);
	}

	[System.Serializable]
	public class senpaiGUI_data {
		[Header("先輩の画像")]
		public Sprite SenpaiHero_img;
		[Header("先輩の全身画像")]
		public Sprite Senpai_img;
		[Header("先輩のプロフィール画像")]
		public Sprite Senpaipro_img;
		[Header("先輩のSelectButton画像")]
		public Sprite SenpaiSelectButton_img;
		[Header("先輩のDeselectButton画像")]
		public Sprite SenpaiDeselectButton_img;
		[Header("ギャラリーの先輩の説明画像"), Space(10)]
		public Sprite GalSenpaiInfo_img;
		[Header("ギャラリーの先輩のイラストボタン画像")]
		public Sprite GalButton_img;
		[Header("ギャラリーの先輩スペシャル画像")]
		public Sprite GallrySenpai_img;
	}
}
