using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatchBall : MonoBehaviour {
	GameObject pitcher;
	Rigidbody rigidbody;

	Vector3 targetPos;
	// Use this for initialization
	void Start () {
		pitcher = GameObject.FindGameObjectWithTag ("Pitcher");
	}

	// Update is called once per frame
	void Update () {
		
	}
}
