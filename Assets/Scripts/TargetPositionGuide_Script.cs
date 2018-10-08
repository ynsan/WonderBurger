using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TargetPositionGuide_Script : MonoBehaviour {


	public RectTransform Is;

	// 受け身
	public GameObject parentTarget;
	//private Transform parentTarget_rect;

	public Transform parentCanvas;

	void Awake (){
		//Is = new Vector3(this, 0, 0);
	}

	void Start () {
		parentCanvas = GameObject.Find("@CharacterPos_GuideCanvas").gameObject.transform;
		//this.gameObject.transform.parent = parentCanvas.transform;
		//parentTarget_rect = parentTarget.transform;
	}

	void Update () {
		//Is.position = RectTransformUtility.WorldToScreenPoint (Camera.main, parentTarget.transform.position);
		//Is.position = new Vector3(parentTarget_rect.position.x, 0, 0);

	}
}
