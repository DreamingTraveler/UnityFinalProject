using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomeRunDetector : MonoBehaviour {

	private GameObject field;
	// Use this for initialization
	void Start () {
		field = GameObject.Find ("Field");
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnCollisionEnter(Collision col){
		int playerOnBaseNum = field.GetComponent<BaseCondition> ().playerOnBaseNum;
		field.GetComponent<Game> ().AddPoint (playerOnBaseNum + 1);
		gameObject.GetComponent<MeshRenderer> ().material.color = Color.red;
		field.GetComponent<BaseCondition> ().SetBase ("Empty");
	}
}
