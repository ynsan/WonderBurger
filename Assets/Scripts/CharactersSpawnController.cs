using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharactersSpawnController : MonoBehaviour {

	[SerializeField]
	public Character aa;
	void Start () {
		
	}

}


public class Character {
	[Header("繰り返しが行われる回数")]
	public int val = 3;

	public float Spawn_time;
}