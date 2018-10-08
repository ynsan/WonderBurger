using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomersController : MonoBehaviour {

	// 注文中の客の数
	[Header("最大注文客数")]
	public int orderCustomers_max = 3;
	[Header("現在の注文客数")]
	public int orderingCustomers_num = 0;

	[Header("客キャラクターの親GameObject")]
	public GameObject CustomersParent;
	public List<GameObject> Customers = new List<GameObject>();

	private float wait_time = 3f;

	void Start () {
		// CustomersParentから,客キャラクター(固定)の取得
		foreach (Transform child in CustomersParent.transform) {
			// Customersリストに追加
			Customers.Add(child.gameObject);
		}

		orderingCustomers_num = 0;

		// 初期オーダータイム
		wait_time = 3f;
	}

	void Update () {
		if (PlayerController.Tutorial == false) {
			if (orderingCustomers_num <= orderCustomers_max) {
				wait_time -= Time.deltaTime;
				if (wait_time <= 0) {
					OrderStart();
					wait_time = Random.Range(0.8f, 1.2f);
				}
			}
		}
	}

	public void OrderStart() {
		Debug.Log("注文入りました！");
		orderingCustomers_num++;
		int rnd = Random.Range(0, Customers.Count);
		Customers[rnd].GetComponent<Customer>().Ordering_bool = true;
	}
}
