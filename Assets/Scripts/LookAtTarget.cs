using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Targetを見るScript
/// </summary>
public class LookAtTarget : MonoBehaviour {

	public Transform isGameObject;

	public bool TargetPlayer = false;
	public Transform Target;

	public bool TargetLookPosition = false;

	[Header("RotateRock XYZ")]
	public bool[] rotate_rock = new bool[3];

	void Awake(){
		if (!isGameObject){
			isGameObject = this.gameObject.transform;
		}
		if (TargetPlayer = true) {
			Target = GameObject.Find ("@PLAYER").gameObject.transform;
		}
	}

	void Update() {
		isGameObject.LookAt(Target);
		if (rotate_rock[0] == true) { // X
									  //isGameObject.transform.rotation = new Vector3();
		}
		if (TargetLookPosition == true) {
			isGameObject.transform.position = new Vector3(Target.position.x, transform.position.y, 2);
		}
	}
}
