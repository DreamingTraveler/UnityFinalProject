using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingleDetector : MonoBehaviour {
	public GameObject field;
	public GameObject pitcher;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnCollisionEnter(Collision col){
		if (field.GetComponent<Game> ().isBallFlying) {
			gameObject.GetComponent<MeshRenderer> ().material.color = Color.green;
			field.GetComponent<BaseCondition> ().BaseStateMachine(1);
			field.GetComponent<Game> ().isHitting = true;
			field.GetComponent<Game> ().ToNextPlayer();
			pitcher.GetComponent<Pitch> ().cloneBall.SetActive (false);
			pitcher.GetComponent<Pitch> ().EnableChooseButton ();
		}
	}
}
