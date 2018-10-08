using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class GameController : MonoBehaviour {

	// ゲーム中のおおよその操作をします

	[Header("ゲームの制限時間")]
	public float gameTime = 60f;

	private int totalScore = 0;
	private int bonusScore = 0;

	[HideInInspector]
	public float gameStart_CountDowm = 3f;

	[Header("スポーン位置の登録")]
	public SpawnPoint_Data[] SpawnPoint_data;

	public int spawnLevel = 0;
	public SpawnControl_Data[] SpawnControl_data;
	public int rnd_spawn = 0;
	public float rnd_spawnTime = 5;

	[Header("スポーン条件なし, 常時")]
	public GameObject[] bg_Spawn;
	public int rnd_bgSpawn = 0;
	public float rnd_bgSpawnTime = 5;

	// UseEnemyWorldCounter
	public int EnemyWorldCount_max = 5;
	public static int EnemyWorldCount = 0;

	public static bool GameAction = false;
	private float gameEnd_tl;
	private float DroneSpawn_tl = 0;

	public GameObject[] Senpai_TextSetData = new GameObject[4];

	public GameObject DroneCustomer;

	[Header("ResultScore")]
	public int[] ResultScores = new int[4];
	public Sprite[] ResultScoreRank_img = new Sprite[4];

	[Header("Skybox")]
	public Skybox bgSky;
	private Material bgSky_mtl;
	private float skyExposure = 2;
	private float skyExposure_timeframeVal;
	public Color32 startSky_c;
	public Color32 endSky_c;

	public GameGUI_Controller _GameGUI_Controller_;
	public GlobalSetting _GlobalSetting_;


	private string Bonus_string = "";

	public Sounds _sounds;

	// チュートリアル用
	private float action_time = 0;
	private int order_val = 0;
	private Customer cus;

	void Awake() {
		Time.timeScale = 1;
		GameAction = true;
		action_time = 0;
		order_val = 0;
	}

	void Start() {
		// Componentのセット処理
		_GlobalSetting_ = GameObject.Find("@DontDestroyOnLoad").gameObject.GetComponent<GlobalSetting>();
		_GameGUI_Controller_ = GameObject.Find("@GUI_Canvas").gameObject.GetComponent<GameGUI_Controller>();
		
		gameStart_CountDowm = 3f;
		bonusScore = 0;
		spawnLevel = 0;
		rnd_spawnTime = Random.Range(SpawnControl_data[spawnLevel].rnd_time.x, SpawnControl_data[spawnLevel].rnd_time.y);
		SpawnControl_data[spawnLevel].isLevelSet_tl = SpawnControl_data[spawnLevel].isLevelSet_time;
		//rnd_spawnTime = SpawnControl_data[spawnLevel].isLevelSet_time;
		DroneSpawn_tl = 0;
		EnemyWorldCount = 0;

		gameTime = 60f;

		// 背景の空の初期設定
		bgSky_mtl = bgSky.material;
		skyExposure = 2;
		//skyExposure_timeframeVal = skyExposure;
		bgSky.material.SetFloat("_Exposure", skyExposure);
		bgSky.material.SetColor("_TintColor", new Color(0.42f, 0.42f, 0.42f, 1));

		// string型の初期化
		Bonus_string = "";
	}


	// Update is called once per frame
	void Update() {

		if (GameAction == true) {
			if (PlayerController.TimeCount == true) {
				if (PlayerController.Tutorial == false)
				gameTime -= Time.deltaTime;
				// 空の変化
				skyExposure -= gameTime / 100000;
				skyExposure -= gameTime / 100000;
				bgSky.material.SetFloat("_Exposure", skyExposure);
				bgSky.material.SetColor("_TintColor", new Color(0.42f + gameTime / 240000, 0.42f, 0.42f, 1));

				if (PlayerController.GamePlay == true) {

					SpawnControl_data[spawnLevel].isLevelSet_tl -= Time.deltaTime;
					rnd_spawnTime -= Time.deltaTime;

					if (PlayerController.Tutorial != true) {

						AutoSpawnControl();

							rnd_bgSpawnTime -= Time.deltaTime;
							if (rnd_bgSpawnTime <= 0) {
								GameObject bgSpawn = Instantiate(bg_Spawn[Random.Range(0, bg_Spawn.Length)], SpawnPoint_data[1].spawnPoints[Random.Range(0, SpawnPoint_data[1].spawnPoints.Length)].position, Quaternion.identity);
								rnd_bgSpawnTime = Random.Range(5, 12);
							}
							// ドローン
							DroneSpawn_tl += Time.deltaTime;
							if (DroneSpawn_tl >= 20f) {
								GameObject bgSpawn = Instantiate(DroneCustomer, SpawnPoint_data[2].spawnPoints[0].position, Quaternion.identity);
								DroneSpawn_tl = 0;
							}
					}
					if (gameTime <= 0) {
						Debug.Log("◆ゲーム終了！◆");
						GameAction = false;
						_GameGUI_Controller_.StopEnd_text.enabled = true;
						PlayerController.GamePlay = true;
						PlayerController.PlayerDamage = false;
						PlayerController.TimeCount = false;
						PlayerController.CanUse_PlayerSkill = false;
						PlayerController.PlayerSkill = false;
					}
				}
			}
			else if (PlayerController.TimeCount == false) {
				gameStart_CountDowm -= Time.deltaTime;
				if (gameStart_CountDowm <= 0) {
					//Debug.Log ("◆ゲーム開始！◆");
					// ゲーム終了時に発生
					//PlayerController.GamePlay = true;
				}
			}

			if (PlayerController.Tutorial == true && gameStart_CountDowm <= 0) {
				// チュートリアル
				Debug.Log("チュートリアル番号" + PlayerController.tutorial_val);

				// 準備
				if (PlayerController.tutorial_val == 0) {
					action_time += Time.deltaTime;
					if (action_time >= 1) {
						PlayerController.tutorial_val++;
						action_time = 0;
					}
				}
				// 視点移動
				if (PlayerController.tutorial_val == 1) {
					action_time += Time.deltaTime;
					if (CameraRotator.GetAxis_bool == true) {
						Debug.Log("Lジョイ");
						//PlayerController.Tu_action = true;
						action_time += Time.deltaTime * 5;  // 動かすと次の表示が早くなる
						if (action_time >= 8) {
							PlayerController.tutorial_val++;
							action_time = 0;
							cus = GameObject.Find("Customer").gameObject.GetComponent<Customer>();
							cus.Ordering_bool = true;
						}
					}
				}
				// お客さんのオーダー
				if (PlayerController.tutorial_val == 2) {
					action_time += Time.deltaTime;
					if (action_time >= 3) {
						PlayerController.tutorial_val++;
						action_time = 0;
					}
				}
				// オーダーを飛ばす
				if (PlayerController.tutorial_val == 3) {
					if (cus.Ordering_bool == false) {
						PlayerController.tutorial_val++;
					}
				}
				// オーダー成功
				if (PlayerController.tutorial_val == 4) {
					action_time += Time.deltaTime;
					if (action_time >= 3) {
						PlayerController.tutorial_val++;
						action_time = 0;
					}
				}
				// オーダー成功2
				if (PlayerController.tutorial_val == 5) {
					action_time += Time.deltaTime;
					if (action_time >= 3) {
						PlayerController.tutorial_val++;
						action_time = 0;
					}
				}
				// 強盗出現
				if (PlayerController.tutorial_val == 6) {
					action_time += Time.deltaTime;
					if (action_time >= 2) {
						action_time = 0;
						GameObject tu_enemy = Instantiate(SpawnControl_data[0].SpawnCharacter_data[0].Character,
						SpawnPoint_data[0].spawnPoints[Random.Range(0, SpawnPoint_data[0].spawnPoints.Length)].position, Quaternion.identity);
						Debug.Log("ごーとー");
						PlayerController.tutorial_val++;
					}
				}
				if (PlayerController.tutorial_val == 7) {
				}

				// 強盗撃破
					if (PlayerController.tutorial_val == 8) {
					action_time += Time.deltaTime;
					if (action_time >= 3) {
						PlayerController.tutorial_val++;
						action_time = 0;
						cus = GameObject.Find("Customer (3)").gameObject.GetComponent<Customer>();
						cus.Ordering_bool = true;
					}
				}
				// デブ
				if (PlayerController.tutorial_val == 9) {


					if (cus.Ordering_bool == false) {
						order_val++;
						cus.Ordering_bool = true;
					}

					if (PlayerController.CanUse_PlayerSkill == true) {
						for (int i = 0; i <= 3; i++) {
							GameObject tu_enemy = Instantiate(SpawnControl_data[0].SpawnCharacter_data[0].Character,
							SpawnPoint_data[0].spawnPoints[Random.Range(0, SpawnPoint_data[0].spawnPoints.Length)].position, Quaternion.identity);
						}
						PlayerController.tutorial_val++;
					}
				}
				// スキル
				if (PlayerController.tutorial_val == 10) {
					if (PlayerController.CanUse_PlayerSkill == false) {
						PlayerController.tutorial_val++;
					}
				}
				// スキル発動
				if (PlayerController.tutorial_val == 11) {
					if (PlayerController.PlayerSkill == true) {
						PlayerController.tutorial_val++;
					}
				}
				// オーダー成功2
				if (PlayerController.tutorial_val == 12) {
					action_time += Time.deltaTime;
					if (action_time >= 3) {
						PlayerController.tutorial_val++;
						action_time = 0;
					}
				}
				// オーダー成功2
				if (PlayerController.tutorial_val == 13) {
					action_time += Time.deltaTime;
					if (action_time >= 3) {
						PlayerController.tutorial_val++;
						action_time = 0;
					}
				}
				// オーダー成功2
				if (PlayerController.tutorial_val == 14) {
					action_time += Time.deltaTime;
					if (action_time >= 5) {
						action_time = 0;
						PlayerController.Tutorial = false;
						// BGMの二重再生防止
						Destroy(GameObject.Find("BGM").gameObject);
						SceneManager.LoadScene("Title_scene");
						SceneManager.LoadScene("Game_scene");
					}
				}
			}
		} else if (GameAction == false) { // ゲーム終了
			GameClear();
		}

	}

	public void Spawn() {
		Debug.Log("すぽーん");

		// 生成するEnemyの配列値をランダム
		rnd_spawn = Random.Range(0, SpawnControl_data[spawnLevel].SpawnCharacter_data.Length);

		if (SpawnControl_data[spawnLevel].SpawnCharacter_data[rnd_spawn].Character.GetComponent<ObjectStatus>().UseEnemyWorldCounter == true
			&& EnemyWorldCount < EnemyWorldCount_max) {
			GameObject sp = Instantiate(SpawnControl_data[spawnLevel].SpawnCharacter_data[rnd_spawn].Character,
				SpawnPoint_data[0].spawnPoints[Random.Range(0, SpawnPoint_data[0].spawnPoints.Length)].position, Quaternion.identity
			);
			// ランダムスポーンタイム
			rnd_spawnTime = Random.Range(SpawnControl_data[spawnLevel].rnd_time.x, SpawnControl_data[spawnLevel].rnd_time.y);
			EnemyWorldCount++;
		}
		//SpawnControl_data [spawnLevel].isLevelSet_tl = rnd_spawnTime;
	}

	public void AutoSpawnControl() {
		if (SpawnControl_data[spawnLevel].isLevelSet_tl >= 0) {
			if (rnd_spawnTime <= 0) {
				Spawn();
			}
		}
		else if (SpawnControl_data[spawnLevel].isLevelSet_tl <= 0 || spawnLevel + 1 >= SpawnControl_data.Length) {
			spawnLevel++;
			SpawnControl_data[spawnLevel].isLevelSet_tl = SpawnControl_data[spawnLevel].isLevelSet_time;
			Debug.Log("LvUP!! " + spawnLevel);
		}
	}


	public void GameClear() {
		// ゲーム終了後のウェイト処理
		if (gameEnd_tl >= 2 && PlayerController.GamePlay == true) {
			Debug.Log("結果！");
			// BGMの二重再生防止
			Destroy(GameObject.Find("BGM").gameObject);
			_sounds.audios[0].Play();
			Bonus_string = "";  // Text初期化
			PlayerController.GamePlay = false;
			_GameGUI_Controller_.StopEnd_text.enabled = false;

			// ボーナス条件
			PlayerController _PlayerController_ = GameObject.Find("@PLAYER").gameObject.GetComponent<PlayerController>();

			// skill未使用Bonus
			if (_PlayerController_.Bonus_noUsedSkill == true)
				bonusScore += 10000;
			Bonus_string += "ノースキルボーナス: + 10000\n";
			// damage0 Bonus
			if (_PlayerController_.Bonus_noDamage == true)
				bonusScore += 5000;
			Bonus_string += "ノーダメージボーナス: + 5000\n";
			totalScore = 0;
			totalScore = _PlayerController_.GetScore + bonusScore;

			// スコアランク
			int rank_ = -1;
			if (totalScore >= ResultScores[0]) {
				rank_ = 0;
			}
			else if (totalScore >= ResultScores[1]) {
				rank_ = 1;
			}
			else if (totalScore >= ResultScores[2]) {
				rank_ = 2;
			}
			else if (totalScore >= ResultScores[3]) {
				rank_ = 3;
			}
			else if (totalScore >= ResultScores[4]) {
				rank_ = 4;
			}
			else if (totalScore < ResultScores[4]) {
				rank_ = 5;
			}

			_GameGUI_Controller_.ResultScoreRank.sprite = ResultScoreRank_img[rank_];
			_GameGUI_Controller_.TotalScore.text = totalScore.ToString();
			_GameGUI_Controller_.Bonus_text.text = Bonus_string;
			_GameGUI_Controller_.firstSelectButton.Select();
			_GameGUI_Controller_.resultSenpai_text.text = _GameGUI_Controller_._SenpaiText_.is_TextSetGroup[4].textSet[rank_];
			_GameGUI_Controller_.ClearCanvas.SetActive(true);

		}
		else {
			gameEnd_tl += Time.deltaTime;
		}
	}

	[System.Serializable]
	public class SpawnPoint_Data {
		public string label;
		public Transform[] spawnPoints;
	}

	[System.Serializable]
	public class SpawnControl_Data {
		[Header("この1セット時間")]
		public float isLevelSet_time = 20f;
		[HideInInspector]
		public float isLevelSet_tl = 20f;
		public Vector2 rnd_time = new Vector2(2, 5);
		[Header("出現するBackNPCのデータ")]
		public SpawnCharacter_Data[] SpawnCharacter_data;
	}
	[System.Serializable]
	public class SpawnCharacter_Data {
		[Header("BackNPC")]
		public GameObject Character;
		public int Max_val;
	}
}