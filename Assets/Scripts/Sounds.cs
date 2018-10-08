using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sounds : MonoBehaviour {

	public GameObject GetSounds_object;
	[HideInInspector]
	public AudioSource[] audios;

	void Start () {
		if (!GetSounds_object)
		GetSounds_object = this.gameObject;

		audios = GetSounds_object.GetComponents<AudioSource>();
	}
}
