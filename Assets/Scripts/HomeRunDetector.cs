using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HomeRunDetector : MonoBehaviour {
	public GameObject field;
	public GameObject pitcher;
    private GameObject judgeHomeRun;
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
		field.GetComponent<Game>().AddHitNum();
		Invoke ("SwitchCamera", 1.5f);
	}

	private void SwitchCamera(){
		field.GetComponent<Game>().isBallCameraMoving = false;
		pitcher.GetComponent<Pitch> ().cloneBall.SetActive (false);
        judgeHomeRun.GetComponent<Pitch> ().judgeHomeRun.enabled = true;
		if (field.GetComponent<Game>().nowAttack == "visitor") {
			pitcher.GetComponent<Pitch> ().EnableReadyBtn ();
			field.GetComponent<SwitchCamera>().SwitchToHitterCamera();
		} else {
			pitcher.GetComponent<Pitch>().EnableChooseButton();
		}
        //field.GetComponent<Game> ().SetSituation("HomeRun");
	}
}
