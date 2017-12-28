﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoubleDetector : MonoBehaviour {
	public GameObject field;
	public GameObject pitcher;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnCollisionEnter(Collision col){
		if (field.GetComponent<Game> ().isBallFlying &&
			GameObject.Find("Hitter").GetComponent<HitBall>().randomY <= 600) {
			gameObject.GetComponent<MeshRenderer> ().material.color = Color.green;
			field.GetComponent<BaseCondition> ().BaseStateMachine(2);
			field.GetComponent<Game> ().isHitting = true;
			field.GetComponent<Game> ().ToNextPlayer();
			field.GetComponent<Game>().isBallFlying = false;
			field.GetComponent<Game>().AddHitNum();
			Invoke ("SwitchCamera", 3.0f);
		}
	}

	private void SwitchCamera(){
		field.GetComponent<Game>().isBallCameraMoving = false;
		pitcher.GetComponent<Pitch> ().cloneBall.SetActive (false);
		if (field.GetComponent<Game>().nowAttack == "visitor") {
			pitcher.GetComponent<Pitch> ().EnableReadyBtn ();
			field.GetComponent<SwitchCamera>().SwitchToHitterCamera();
		} else {
			pitcher.GetComponent<Pitch>().EnableChooseButton();
		}
	}
}
