using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI_Controller : MonoBehaviour {

	// プレイヤーのターゲット
	public GameObject Player;

	public float isMoveSpeed = 0.03f;

	public bool goTargetForwardMove = false;
	public bool goTarget = true;

	public Vector3 VelocityLinearMove = new Vector3(0, 0, 0);
	private float VelocityLinearMove_AwakeTime = 0;
	private float VelocityLinearMove_waitTime = 0;

	public bool canJump = false;

	[HideInInspector]
	public Rigidbody RB;

	void Start () {
		Player = GameObject.Find("@PLAYER").gameObject;
		RB = this.gameObject.GetComponent<Rigidbody>();
	}
	
	void Update () {
		if (goTargetForwardMove == true) {
			//if (goTarget == true) {
				Quaternion targetRotation = Quaternion.LookRotation (Player.transform.position - transform.position);
				transform.rotation = Quaternion.Slerp (transform.rotation, targetRotation, Time.deltaTime * 30);
			//} else {
			//	Quaternion targetRotation = Quaternion.LookRotation (-Player.transform.position * 1.2f - transform.position);
			//	transform.rotation = Quaternion.Slerp (transform.rotation, targetRotation, Time.deltaTime * 30);
			//}
			RB.AddForce(Vector3.forward * isMoveSpeed * -32);
			transform.position += transform.forward * isMoveSpeed;
		}

		else if (this.gameObject.CompareTag("Customer")) {
			VelocityLinearMove_AwakeTime += Time.deltaTime;
			if (VelocityLinearMove_AwakeTime >= 1) {
				VelocityLinearMove_waitTime += Time.deltaTime;
			} else {
				this.transform.position += transform.up * isMoveSpeed * Time.deltaTime;
			}
			if (VelocityLinearMove_waitTime >= 10) {
				this.transform.position += transform.up * isMoveSpeed * Time.deltaTime;
			}
		} else {
			this.transform.position += transform.up * isMoveSpeed * Time.deltaTime;
		}

		//RB.AddForce (transform.forward * isMoveSpeed);
	}

	void OnCollisionEnter(Collision col){
		if (col.gameObject && canJump == true) {
			JumpAction ();
		}
		if (col.gameObject.CompareTag("Bullet") && goTarget != false) {
			goTarget = false;
			isMoveSpeed *= -1;
			this.transform.eulerAngles *= -1; 
		}
	}

	public void JumpAction(){
		RB.AddForce(this.transform.up * 120f);
	}
}
