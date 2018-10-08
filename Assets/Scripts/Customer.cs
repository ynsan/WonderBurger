using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Customer : MonoBehaviour {

	[Header("注文中Bool")]
	public bool Ordering_bool = false;
	public Transform Neck_Object;
	public LookAtTarget Head_Object;
	public bool isSpecialCustomer = false;
	private float is_LifeTime = 500;
	private float VelocityLinearMove_AwakeTime = 0;
	private float VelocityLinearMove_waitTime = 0;

	public GameObject isParentGameObject = null;

	public GameObject OrderItem;

	public GameObject[] OrderItemData;
	public Sprite[] OrderItemSprites;
	public Color32[] baseGauge_colors = new Color32[3];

	private Vector3 header = new Vector3(0, 1, 0);

	[Header("ゲージの現在値")]
	public int Gauge_val = 0;

	public int OrderGauge_Score = 100;
	public int OrderClear_Score = 500;

	// 
	public GameObject OrderClear_Particle;
	public GameObject OrderMiss_Particle;

	// パーティクルの生成制限をかけるタイム
	private float particle_waittime = 0.4f;

	public GameObject positionGuide;
	public Transform positionGuide_parent;

	public Animator animator;

	public CustomersController _CustomersController_;
	public PlayerController _PlayerController_;
	public TextSet _SenpaiText_;

	// UGUI
	[Header("Scriptファイルへ直接, custmer_state プレハブをアタッチする")]
	public Canvas cusState_canvas;
	/// <summary>
	/// アイテムのSpriteを使用
	/// </summary>
	public Image StateGauge;
	public Image[] baseGauge = new Image[3];

	public int SetGauge_val = 3;

	private float GaugeFillAmount_val;

	private bool isRendered_camera = false;



	void Start() {

		if (isSpecialCustomer != true) {
			this.gameObject.transform.parent = GameObject.Find("@Customers").gameObject.transform;
			Neck_Object = GameObject.Find(this.name + "Model@bottom01/Skelton_Root/HipJoint_Root/spine/Model@top01__/Skelton_Root/TopRoot_waist/breast/neck").gameObject.transform;
			Head_Object = GameObject.Find(this.name + "Model@bottom01/Skelton_Root/HipJoint_Root/spine/Model@top01__/Skelton_Root/TopRoot_waist/breast/neck/HeadParts").GetComponent<LookAtTarget>();
		}

		_CustomersController_ = GameObject.Find("@GameEventsController").gameObject.GetComponent<CustomersController>();
		_PlayerController_ = GameObject.Find("@PLAYER").gameObject.GetComponent<PlayerController>();
		_SenpaiText_ = GameObject.Find("@DontDestroyOnLoad").gameObject.GetComponent<GlobalSetting>().CharacterData[GlobalSetting.Senpai_ID]._senpaiTextSet_;

		// このコンポーネントのGameObjectの親GameObject
		if (isParentGameObject == null) {
			isParentGameObject = this.gameObject;
		}

		OrderRandom();
		Set_CustmerState();

		header = new Vector3(transform.position.x, transform.position.y + 2.4f, transform.position.z);

		Gauge_val = 0;

		// 色をScriptから入力
		baseGauge_colors[0] = new Color32(0, 255, 0, 255);      // Humbger
		baseGauge_colors[1] = new Color32(255, 0, 0, 255);      // Poteto
		baseGauge_colors[2] = new Color32(255, 255, 0, 255);     // Drink

		// positionGuideを作成
		/*
		if (positionGuide) {
			GameObject pusGuide = Instantiate(positionGuide, new Vector3(0,0,2), Quaternion.identity);
			pusGuide.GetComponent<TargetPositionGuide_Script>().parentTarget = this.gameObject;
			// 親子化
			pusGuide.transform.parent = positionGuide_parent;
			positionGuide.SetActive (false);
		}
		*/
	}

	void Update() {
		
		if (isSpecialCustomer == true) {
			is_LifeTime -= Time.deltaTime;
			is_LifeTime--;
			if (is_LifeTime <= 0f) {
				Destroy(isParentGameObject);
				_CustomersController_.orderingCustomers_num--;
			}

			VelocityLinearMove_AwakeTime += Time.deltaTime;
			if (VelocityLinearMove_AwakeTime >= 0.12f) {
				VelocityLinearMove_waitTime += Time.deltaTime;
			}
			else {
				this.transform.position += transform.forward * 6.4f * Time.deltaTime;
				this.transform.position += transform.up * 2 * Time.deltaTime;
			}
			if (VelocityLinearMove_waitTime >= 300) {
				this.transform.position += transform.up * Time.deltaTime;
			}
			//this.transform.position += transform.up * 4 * Time.deltaTime;
		}
		else {
			// 通常の客
			animator.SetBool("is_Ordering", Ordering_bool);
			Head_Object.enabled = Ordering_bool;
		}
		if (!isSpecialCustomer && Ordering_bool == false) {
			Head_Object.transform.rotation = Neck_Object.rotation;
		}

		if (Ordering_bool == true && cusState_canvas.enabled == false) {
			OrderRandom();
			cusState_canvas.enabled = Ordering_bool;
		}

		if (Ordering_bool == true && StateGauge.fillAmount <= 1) {
			cusState_canvas.enabled = Ordering_bool;
			StateGauge.fillAmount = GaugeFillAmount_val * Gauge_val;

			if (SetGauge_val <= Gauge_val) { //StateGauge.fillAmount >= 1) {
											 // 注文クリア処理
				_PlayerController_.GetScore += OrderClear_Score;
				_PlayerController_.clearOrder_point++;
				Ordering_bool = false;

				Instantiate(OrderClear_Particle, header, this.gameObject.transform.rotation);
				Gauge_val = 0;
				if (isSpecialCustomer != true) {
					_CustomersController_.orderingCustomers_num--;
				}
			}
		}
		else {
			cusState_canvas.enabled = Ordering_bool;
		}

		if (PlayerController.PlayerSkill == true) {
			Debug.Log("スキル受け取り");
			Gauge_val += SetGauge_val;
			_PlayerController_.GetScore += OrderGauge_Score * SetGauge_val;
		}

		//if (particle_waittime <= 0.4f){
		//	particle_waittime += Time.deltaTime;
		//}
		if (particle_waittime >= 0) {
			particle_waittime -= Time.deltaTime;
		}

		//
		if (isRendered_camera == false) {
		}
		isRendered_camera = false;
		//positionGuide.SetActive (!false);
	}

	// ゲージの設定処理
	public void Set_CustmerState() {
		//if (_CustomersController_.orderingCustomers_num <= _CustomersController_.orderCustomers_max){
		cusState_canvas.enabled = true;
		// FillAmountのゲージの1メモリの作成
		GaugeFillAmount_val = 1f / SetGauge_val;
		//}

	}

	// メインカメラに映った時だけ
	// 見えないSkinnedMeshRendererをアタッチしていること
	private void OnWillRenderObject() {
		if (Camera.current.CompareTag("MainCamera")) {
			Debug.Log(this.gameObject.name + "_isRendered_camera_true");
			isRendered_camera = true;
		}

	}

	public void OrderRandom() {
		int rnd = Random.Range(0, OrderItemData.Length - 1);
		OrderItem = OrderItemData[rnd];
		StateGauge.sprite = OrderItemSprites[rnd];
		baseGauge[0].sprite = StateGauge.sprite;
		baseGauge[1].sprite = StateGauge.sprite;
		// ベースのアウトライン色の変更
		baseGauge[1].color = baseGauge_colors[rnd];

		if (positionGuide) {
			/*
			GameObject pusGuide = Instantiate(positionGuide, new Vector3(0,0,2), Quaternion.identity);
			pusGuide.GetComponent<TargetPositionGuide_Script>().parentTarget = this.gameObject;
			// 親子化
			pusGuide.transform.parent = positionGuide_parent;
			*/
		}
	}

	void OnTriggerEnter(Collider col) {
		if (PlayerController.GamePlay == true) {
			if (col.gameObject.CompareTag("Bullet") && Ordering_bool == true) {
				if (OrderItem.name == col.gameObject.name) {
					Debug.Log("受け取り");
					if (isSpecialCustomer == true) {
						VelocityLinearMove_waitTime = 1200f;
					}
					Gauge_val++;
					_PlayerController_.GetScore += OrderGauge_Score;
					SenpaiText.Senpai_text = _SenpaiText_.is_TextSetGroup[0].textSet[Random.Range(0, _SenpaiText_.is_TextSetGroup[0].textSet.Length)];
				}
				else {
					// 注文ミス
					if (particle_waittime <= 0) {
						SenpaiText.Senpai_text = _SenpaiText_.is_TextSetGroup[2].textSet[3];
						particle_waittime = 0.4f;
						Instantiate(OrderMiss_Particle, header, this.gameObject.transform.rotation);
					}
				}
				Destroy(col.gameObject);
			}
		}
	}
}
