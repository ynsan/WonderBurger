using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {

	[Header("Bullet_NameTag")]
	public string is_NameTag = "";

	[Header("ParentCharacter")]
	// 弾を撃ったキャラから受け取るTag
	public string char_tag = "";

	public GameObject BulletModel;

	[Space(3)]
	public bool colDestroy = true;
	//public bool colGround_stop = ;

	public int AttackPower = 10;
	public float Speed = 0.2f;

	/// <summary>
	/// 弾の発射最小頻度 押しっぱなし用
	/// </summary>
	public float shot_frequencyTime = 0.2f;
	public float destroy_time = 20f;

	public GameObject colEffect;
	public GameObject GroundedEffect;

	private Rigidbody RB;

	void Awake() {
		destroy_time = 80f;
		if (this.GetComponent<Rigidbody> ()) {
			RB = this.GetComponent<Rigidbody> ();
		}
	}

	void OnEnable () {
		if (RB) {
			RB.velocity = new Vector3 (0, 1.6f, Speed / 4);
			RB.constraints = RigidbodyConstraints.None;
		}
	}

	void Update () {

		destroy_time -= Time.deltaTime;
		destroy_time--;

		if (destroy_time <= 0f){
			Destroy (this.gameObject);
		}

		transform.position += transform.forward * Speed;
		BulletModel.transform.Rotate(new Vector3(Random.Range(0, 180), Random.Range(0, 180), Random.Range(0, 180)) * Time.deltaTime * (Speed * 8));
	}

	void OnCollisionEnter(Collision col){
		if (col.gameObject && colEffect){
			GameObject colEff = Instantiate (colEffect, this.transform.position, Quaternion.identity);
		}
		if (col.gameObject.CompareTag("Enemy") && colDestroy == true && PlayerController.Tutorial == false){
			SenpaiText.Senpai_text = GameObject.Find ("@DontDestroyOnLoad").gameObject.GetComponent<GlobalSetting>().CharacterData [GlobalSetting.Senpai_ID]._senpaiTextSet_.is_TextSetGroup[1].textSet[0];
			Destroy (this.gameObject);
		}

		if (col.gameObject.CompareTag("Ground")){
			Speed = 0;
			if (GroundedEffect){
				//GameObject groundEff = Instantiate (GroundedEffect, col.gameObject.transform, Quaternion.identity);
				GameObject groundEff = Instantiate (GroundedEffect, this.transform.position, Quaternion.identity);
				if (is_NameTag == "drink") {
					// こぼれるかどうか
					int spil_rnd = Random.Range (0, 1);
					if (spil_rnd == 1) {
							groundEff.transform.localScale = new Vector3 (1, Random.Range (1f, 1.5f), Random.Range (1f, 1.5f));
					}
				}
			}
		}
	}
}
