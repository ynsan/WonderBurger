using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectStatus : MonoBehaviour {

	[Header("Game中の強制削除")]
	public bool is_notGamingDestroy = true;

	[Header("Playerスキルの影響を受けるかどうか")]
	public bool is_SkillDestroy = true;

	public int is_Score = 0;
	public int is_HP = 20;

	[Header("スポーン許可")]
	public bool isSpawn = false;
	public int spawnPoint_ID = -1;

	[Header("ワールド上のEnemy数の制限を受けるかどうか"), SerializeField]
	public bool UseEnemyWorldCounter = true;

	public GameObject hit_particle;
	public GameObject destroy_particle;

	// signulBool
	public static bool EnemyDestroy = false;

	void Start () {
		EnemyDestroy = false;
	}

	void Update () {

		// Playerの後ろへ消える
		if (this.transform.localPosition.z <= -2 || this.transform.localPosition.y <= -10){
			Destroy (this.gameObject);
		}

		if (PlayerController.PlayerSkill == true && is_SkillDestroy == true){
			Destroy (this.gameObject);
			GameObject.Find("@PLAYER").gameObject.GetComponent<PlayerController>().GetScore += is_Score;
		}

		// Game動作が完全停止
		if (is_notGamingDestroy && PlayerController.GamePlay == false) {
			Destroy (this.gameObject);
		}
	}

	void OnCollisionEnter(Collision col){
		Debug.Log ("弾消滅");
		if (col.gameObject.CompareTag("Bullet") && col.gameObject.GetComponent<Bullet>().char_tag == "Player"){
			Debug.Log ("あたり");
			Bullet bullet = col.gameObject.GetComponent<Bullet>();
			is_HP -= bullet.AttackPower;
			GameObject.Find("@PLAYER").gameObject.GetComponent<PlayerController>().GetScore += is_Score;
			if (is_HP <= 0){
				Destroy (this.gameObject);
				// チュートリアル時
				if (PlayerController.Tutorial == true) {
					if (PlayerController.tutorial_val == 7)
					PlayerController.tutorial_val++;
				}
				// Enemyの場合
				if (this.gameObject.CompareTag("Enemy") && EnemyDestroy != true) {
					EnemyDestroy = true;
				}
				if (destroy_particle) {
					GameObject obj = Instantiate(destroy_particle, col.transform.position, Quaternion.identity);
				}
				if (UseEnemyWorldCounter == true) {
					GameController.EnemyWorldCount--;
				}
			}

			if (hit_particle) {
				GameObject obj = Instantiate (hit_particle, col.transform.position, Quaternion.identity);
			}
		}
	}
}
