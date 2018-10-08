using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EasyDestroy : MonoBehaviour {

	// 削除処理を行うScript

	public GameObject isGameObjectParent;

	public bool LifeTime_Destroy = true;
	public float is_LifeTime = 5f;
	public bool OnCollision_Destroy = false;
	public bool OnCollider_Destroy = false;

	void Start () {
		if (isGameObjectParent == null){
			isGameObjectParent = this.gameObject;
		}
	}

	void Update () {
		is_LifeTime -= Time.deltaTime;
		is_LifeTime--;
		if (is_LifeTime <= 0f){
			Destroy(isGameObjectParent);
		}
	}

	void OnCollidionEnter(Collision col){
		Destroy(isGameObjectParent);
	}

	void OnTriggerEnter(Collider col){
		Destroy(isGameObjectParent);
	}
}
