using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomeRunDetector : MonoBehaviour {
	public GameObject field;
	public GameObject pitcher;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnCollisionEnter(Collision col){
		int playerOnBaseNum = field.GetComponent<BaseCondition> ().playerOnBaseNum;
		field.GetComponent<Game> ().AddPoint (playerOnBaseNum + 1);
		gameObject.GetComponent<MeshRenderer> ().material.color = Color.red;
		field.GetComponent<BaseCondition> ().SetBase ("Empty");
		field.GetComponent<Game> ().ToNextPlayer();
		field.GetComponent<Game>().isBallFlying = false;
		Invoke ("SwitchToPitcherCamera", 1.5f);
	}

	private void SwitchToPitcherCamera(){
		field.GetComponent<Game>().isBallCameraMoving = false;
		pitcher.GetComponent<Pitch> ().cloneBall.SetActive (false);
		pitcher.GetComponent<Pitch> ().EnableChooseButton ();
        field.GetComponent<Game> ().SetSituation("HomeRun");
	}
}
