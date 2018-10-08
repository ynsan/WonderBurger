using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameGUI_Controller : MonoBehaviour {

	// ゲーム画面のGUI

	public Text Time_tex;
	public Text Score_tex;
	public Text StartCountDown_text;

	public Transform bullet_buttonsParent;
	public Button[] Bullet_SelectBottons;

	[Header("ゲーム終了時_UI"), Space(12)]
	public Text StopEnd_text;
	public GameObject ClearCanvas;
	public Button firstSelectButton;
	public Image ResultScoreRank;

	public Text TotalScore;

	public Text Clear_Score_tex;
	// Bonusの詳細情報
	public Text Bonus_text;
	public Image resultChar_img;

	[Space(12)]

	public Image Display_DamageEffect;

	public Image Senpai_faceGraphic;
	public Image Senpai_GuideWindow;

	public Image ShotButton_OverImage;

	public GameObject SkillButton;
	public Image SkillGauge_img;
	private float SkillGauge_fillA;

	public Text show_TutorialGuide_text;
	public Text resultSenpai_text;

	[Space(12)]


	public PlayerController _PlayerController_;
	public GameController _GameController_;
	public GlobalSetting _GlobalSetting_;
	public TextSet _SenpaiText_;

	private float private_tl = 0;
	private float damage_tl = 2;

	private bool BulletBotton_bool = false;


	void Awake() {

		StartCountDown_text.enabled = true;
		StopEnd_text.enabled = false;
		// チュートリアルの表記
		show_TutorialGuide_text.enabled = PlayerController.Tutorial;
	}

	void Start () {
		_PlayerController_ = GameObject.Find ("@PLAYER").gameObject.GetComponent<PlayerController>();
		_GameController_ = GameObject.Find ("@GameEventsController").gameObject.GetComponent<GameController>();
		_GlobalSetting_ = GameObject.Find ("@DontDestroyOnLoad").gameObject.GetComponent<GlobalSetting>();
		_SenpaiText_ = _GlobalSetting_.CharacterData [GlobalSetting.Senpai_ID]._senpaiTextSet_;

		// ボタンの引数
		for(int i = 0; i < Bullet_SelectBottons.Length; i++){
			//Button button = Instantiate(Bullet_SelectBottons[i], bullet_buttonsParent);
			int Bullet_argumentVal = i;//一時変数
			Bullet_SelectBottons[i].onClick.AddListener(() => {bulletButton(Bullet_argumentVal);});
		}

		ClearCanvas.SetActive (false);

		Score_tex.text = "" + _PlayerController_.GetScore;
		Senpai_GuideWindow.sprite = _GlobalSetting_.CharacterData[GlobalSetting.Senpai_ID].Senpai_images[0];

		private_tl = 0;

		SkillGauge_fillA = SkillGauge_img.fillAmount / 10;
		SkillGauge_img.fillAmount = 0;

		resultChar_img.sprite = _GlobalSetting_.CharacterData[GlobalSetting.Senpai_ID].Senpai_images2;

	}

	void Update() {

		if (GameController.GameAction == true) {

			Score_tex.text = (_PlayerController_.GetScore).ToString();
			Time_tex.text = ((int)_GameController_.gameTime).ToString();

			if (_GameController_.gameTime <= 10) {
				Time_tex.color = new Color (255, 0, 0);
				Debug.Log (((int)_GameController_.gameTime).ToString ());
			} else {
				Time_tex.color = new Color (255, 255, 255);
			}

			if (PlayerController.GamePlay == false) {
				StartCountDown_text.text = ((int)_GameController_.gameStart_CountDowm + 1).ToString ();
			}
			if (_GameController_.gameStart_CountDowm <= 0 && StartCountDown_text.enabled != false) {
				StartCountDown_text.text = "Start";
				private_tl += Time.deltaTime;
				if (private_tl >= 1.5f) {
					Debug.Log ("◆ゲーム開始！◆");
					StartCountDown_text.enabled = false;
					PlayerController.GamePlay = true;
					PlayerController.TimeCount = true;		//Debug.Log (_SenpaiText_.is_TextSetGroup [2].textSet [0]);
					SenpaiText.Senpai_text = _SenpaiText_.is_TextSetGroup[2].textSet[0];
				}
			}

			if (PlayerController.Tutorial == false) {
				// プレイヤーのダメージ処理
				if (PlayerController.PlayerDamage == true) {
					Display_DamageEffect.color = new Color32(1, 0, 0, 1);
					Display_DamageEffect.gameObject.SetActive(true);
					Senpai_faceGraphic.sprite = _GlobalSetting_.CharacterData[GlobalSetting.Senpai_ID].Senpai_images[1];
				}
				if (Display_DamageEffect.gameObject.activeSelf == true) {
					damage_tl -= Time.deltaTime;
					Display_DamageEffect.color = new Color(1, 0, 0, damage_tl);
					if (Display_DamageEffect.color.a <= 0) {
						PlayerController.PlayerDamage = false;
						Senpai_faceGraphic.sprite = _GlobalSetting_.CharacterData[GlobalSetting.Senpai_ID].Senpai_images[0];
						Display_DamageEffect.gameObject.SetActive(PlayerController.PlayerDamage);
						damage_tl = 2;
					}
				}
			}
			if (SkillGauge_img.fillAmount <= 1) {
				SkillGauge_img.fillAmount = SkillGauge_fillA * _PlayerController_.clearOrder_point;
				SkillButton.SetActive (PlayerController.CanUse_PlayerSkill);
			} else {
				SkillGauge_img.fillAmount = 1;
			}

			// チュートリアル
			if (PlayerController.Tutorial == true) {
				if (PlayerController.tutorial_val == 0)
				SenpaiText.Senpai_text = "チュートリアルを開始します。";
				if (PlayerController.tutorial_val == 1)
					SenpaiText.Senpai_text = "お店にお客さんがいます。\n左ジョイスティックで見回してみましょう。";
				if (PlayerController.tutorial_val == 2)
					SenpaiText.Senpai_text = "中央にいるお客さんに注文アイコンが表示されています。";
				if (PlayerController.tutorial_val == 3)
					SenpaiText.Senpai_text = "注文と同じものをお客さんに投げ渡してみましょう。\nハンバーガーは『B』、ポテトは『A』、ジュースは『X』ボタンで飛ばせます。";
				if (PlayerController.tutorial_val == 4)
					SenpaiText.Senpai_text = "おめでとう！お客さんは喜んでくれました。";
				if (PlayerController.tutorial_val == 5)
					SenpaiText.Senpai_text = "1つの注文をクリアすると、\n『画面下のスコア』と『画面右下にある青いスキルゲージ』が増えます！";
				if (PlayerController.tutorial_val == 6)
					SenpaiText.Senpai_text = "あ！遠くから強盗が…！\nいずれかのアイテムを投げつけて\n強盗を撃退しましょう！！";
				if (PlayerController.tutorial_val == 8)
					SenpaiText.Senpai_text = "おめでとう！強盗はいなくなりました。\n強盗を倒すことでもスコアをかせぐことができます。";
				if (PlayerController.tutorial_val == 9)
					SenpaiText.Senpai_text = "たくさん食べそうなお客さんから注文が入りました。\n画面右下の青いゲージを満タンまでためてみましょう！";
				if (PlayerController.tutorial_val == 10)
					SenpaiText.Senpai_text = "ゲージを満タンになりました！\n必殺技！『Y』ボタンを押して\nスキルを使用してみましょう！";
				if (PlayerController.tutorial_val == 11)
					SenpaiText.Senpai_text = "スキルを発動しました！\n敵もオーダーもクリアされ、スコアも加算されます！\n同時にスキルゲージもリセットされます。";
				if (PlayerController.tutorial_val == 12)
					SenpaiText.Senpai_text = "お疲れさまでした！チュートリアルは以上になります！";
				if (PlayerController.tutorial_val == 13)
					SenpaiText.Senpai_text = "本番では、もっと遠くに風船やドローンなどがやってきます。\n高得点を狙えるチャンスです！";
				if (PlayerController.tutorial_val == 14)
					SenpaiText.Senpai_text = "では…\n一度ゲームをリセットして、\nさっそく本番のゲームを始めてみましょう！！";
			}

		} else if (GameController.GameAction == false && PlayerController.TimeCount == false) {
			Clear_Score_tex.text = "売上スコア：" + (_PlayerController_.GetScore).ToString();
		}
	}

	public void ShotButton_(){
		if (BulletBotton_bool == true && ShotButton_OverImage.rectTransform.localScale.x > 0) {
			ShotButton_OverImage.rectTransform.localScale -= new Vector3 (0.01f, 0.01f, 0);
		} else {
			ShotButton_OverImage.rectTransform.localScale = new Vector3 (0, 0, 0);
			_PlayerController_.SkillAction ();
			ShotButton_Up ();
		}
	}
	public void ShotButton_Down() {
		BulletBotton_bool = true;
	}
	public void ShotButton_Up() {
		ShotButton_OverImage.rectTransform.localScale = new Vector3 (1, 1, 1);
		BulletBotton_bool = false;
	}

	public void bulletButton(int Bullet_argumentVal){
		_PlayerController_.SetBullet_int = Bullet_argumentVal;
	}
}