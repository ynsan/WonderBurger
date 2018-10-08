using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour{
	
	public Camera m_cam;
	public Transform BulletPoint;

	public static bool GamePlay = false;
	public static bool TimeCount = false;
	public static bool PlayerDamage = false;
	public static bool CanUse_PlayerSkill = false;
	public static bool PlayerSkill = false;
	public static bool Tutorial = false;
	public static bool Tu_action;
	public static int tutorial_val = 0;

	public int SetBullet_int = 0;
	public float shot_frequencyTime = 0;
	public GameObject[] use_Bullets = new GameObject[2];
	public GameObject SkillMotionObject;

	[Header("ダメージ時のエフェクト")]
	public GameObject Damage_ParticleEffects;
	public GameObject Skill_ParticleEffects;

	public int GetScore = 0;
	public int clearOrder_point = 0;

	[HideInInspector]
	//public int skillAction_val = 0;
	public bool Bonus_noUsedSkill = true;
	[HideInInspector]
	public bool Bonus_noDamage = true;

	private float invincible_time = 0;
	private bool Invincible = false;

	private int select_num = 0;

	private bool connected = false;

	void Start () {
		GamePlay = false;
		TimeCount = false;
		PlayerDamage = false;
		CanUse_PlayerSkill = false;
		PlayerSkill = false;
		Invincible = false;
		tutorial_val = 0;

		SetBullet_int = 0;
		GetScore = 0;
		clearOrder_point = 0;

		shot_frequencyTime = 0;

		if (DebugGameVersion.DebugMode == true) {
			Tutorial = false;
		}

		// ボーナス初期設定
		Bonus_noUsedSkill = true;
		Bonus_noDamage = true;

		//StartCoroutine (CheckForControllers());
	}

	/*
	IEnumerator CheckForControllers () {
		while (true) {
			var controllers = Input.GetJoystickNames ();

			if (!connected && controllers.Length > 0) {
				Debug.Log ("Connected");
				connected = true;
			} else if (connected && controllers.Length == 0) {
				Debug.Log ("Disconnected");
				connected = false;
			}
			yield return new WaitForSeconds (1f);
		}
	}
	*/


	void Update () {
		if (GameController.GameAction == true) {
			
			if (GetScore <= 0) {
				GetScore = 0;
			}
			if (GamePlay == true) {
				// 注文アイテム切り替え
				if (Input.GetButton ("Fire2") && !(Input.GetButtonDown ("Fire1") || Input.GetButtonDown ("Fire3"))) {
					SetBullet_int = 0;
					BulletButton ();
				}
				if (Input.GetButton ("Fire1") && !(Input.GetButtonDown ("Fire2") || Input.GetButtonDown ("Fire3"))) {
					SetBullet_int = 1;
					BulletButton ();
				}
				if (Input.GetButton ("Fire3") && !(Input.GetButtonDown ("Fire1") || Input.GetButtonDown ("Fire2"))) {
					SetBullet_int = 2;
					BulletButton ();
				}
				if (Input.GetButtonDown ("Fire4") && CanUse_PlayerSkill == true) {
					Debug.Log ("スキル");
					SkillAction ();
				} else {
					PlayerSkill = false;
				}
			}

			// 無敵
			if (Invincible == true) {
				invincible_time += Time.deltaTime;
				if (invincible_time >= 5) {
					Invincible = false;
					invincible_time = 0;
				}
			}

			if (shot_frequencyTime >= 0) {
				shot_frequencyTime -= Time.deltaTime;
			}

			// skill可能
			if (clearOrder_point >= 10) {
				CanUse_PlayerSkill = true;
				clearOrder_point = 10;
			}

			if (GamePlay == false && this.GetComponent<Collider> ().enabled == true) {
				this.GetComponent<Collider> ().isTrigger = false;
			} else if (GamePlay == true && this.GetComponent<Collider> ().enabled == false) {
				this.GetComponent<Collider> ().isTrigger = true;
			}
		}
	}

	void OnCollisionEnter (Collision col){
		if(col.gameObject.CompareTag("Enemy") && Invincible == false){
			Debug.Log ("!!!");
			if (Bonus_noDamage == true)
			Bonus_noDamage = false;
			GameObject coins = Instantiate (Damage_ParticleEffects, col.gameObject.transform.position,
				new Quaternion(Random.Range(0, 360),Random.Range(0, 360),Random.Range(0, 360), Random.Range(0, 360)));
			PlayerDamage = true;
			Invincible = true;
			GetScore -= 100;
			Destroy (col.gameObject);
		}
	}

	public void BulletButton() {
		if (PlayerDamage != true && shot_frequencyTime <= 0) {
			GameObject bullet = Instantiate (use_Bullets[SetBullet_int], BulletPoint.position, m_cam.transform.rotation);
			Bullet bullet_  = bullet.GetComponent<Bullet> ();
			bullet_.char_tag = "Player";
			shot_frequencyTime = bullet_.shot_frequencyTime;
			bullet.name = use_Bullets[SetBullet_int].name;
		}
	}

	public void SkillAction(){
		Debug.Log ("スキル発動!!");

		if (Bonus_noUsedSkill == true)
		Bonus_noUsedSkill = false;

		PlayerSkill = true;
		CanUse_PlayerSkill = false;
		// skill gameObject
		GameObject skill_obj = Instantiate (SkillMotionObject);
		skill_obj.transform.parent = m_cam.gameObject.transform;
		skill_obj.transform.position = new Vector3(0, 0, 0);
		//
		clearOrder_point = 0;
		GameObject skill_eff = Instantiate (Skill_ParticleEffects, BulletPoint.position, Quaternion.identity);
		skill_eff.transform.parent = BulletPoint.transform;
	}
}