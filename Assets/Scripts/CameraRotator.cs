using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRotator : MonoBehaviour {

	public static bool GetAxis_bool;

	private readonly Vector2 AngleLimit = new Vector2(32f, 90f);
	private readonly Vector2 RotationSpeed= new Vector2(45f , 80f);

	private readonly float is_Reverse = -1.0f;// true: -1.0f, false: 1.0f


	private Vector2 currentAngle;
	//private float currentAngleX;
	private Vector3 initEulerAngles;
	private bool isInit = false;


	// カメラオブジェクトを格納する変数
	public GameObject mainCamera;
	// カメラの回転速度を格納する変数
	public Vector2 rotationSpeed = new Vector2(2, 2);
	// マウス移動方向とカメラ回転方向を反転する判定フラグ
	public bool reverse;
	// マウス座標を格納する変数
	private Vector2 lastMousePosition;

	// rayが届く範囲
	public float distance = 120f;

	public GameObject RotateGuide;


	void Start () {
		//startRotation = this.transform.rotation.eulerAngles.y;
		init();
	}

	private float inputValueH;
	private float inputValueV;
	void Update() {

		if (isInit) {
			inputValueH = Input.GetAxis("Horizontal");
			inputValueV = Input.GetAxis("Vertical");

			currentAngle.y += is_Reverse * inputValueH * (RotationSpeed.y * -1) * Time.deltaTime;
			currentAngle.x += is_Reverse * inputValueV * RotationSpeed.x * Time.deltaTime;

			var deltaY = currentAngle.y - initEulerAngles.y;
			if (deltaY > AngleLimit.y) {
				currentAngle.y = initEulerAngles.y + AngleLimit.y;
			} else if (deltaY < -AngleLimit.y) {
				currentAngle.y = initEulerAngles.y - AngleLimit.y;
			}

			var deltaX = currentAngle.x - initEulerAngles.x;
			if (deltaX > AngleLimit.x) {
				currentAngle.x = initEulerAngles.x + AngleLimit.x;
			} else if (deltaX < -AngleLimit.x) {
				currentAngle.x = initEulerAngles.x - AngleLimit.x;
			}

			if (Mathf.Abs(Input.GetAxis("Horizontal")) > 0 && Mathf.Abs(Input.GetAxis("Vertical")) > 0) {
				GetAxis_bool = true;
			} else {
				GetAxis_bool = false;
			}
			//Debug.Log("[input] " + inputValue + " [current angle y] " + currentAngleY);

			mainCamera.transform.localEulerAngles = new Vector3(currentAngle.x, currentAngle.y, initEulerAngles.z);
			RotateGuide.transform.rotation = new Quaternion(mainCamera.transform.rotation.x, 0, 0, mainCamera.transform.rotation.w);
		}
	}

	#region Private methods
	private void init() {
		if (mainCamera == null) {
			mainCamera = GameObject.Find("MainCamera").gameObject;
		}

		if (mainCamera != null) {
			initEulerAngles = mainCamera.transform.localEulerAngles;
			currentAngle.y = initEulerAngles.y;
			currentAngle.x = initEulerAngles.x;

			isInit = true;
		}
	}
	#endregion

}
  