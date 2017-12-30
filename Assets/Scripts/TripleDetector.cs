using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TripleDetector : MonoBehaviour {
	public GameObject field;
	public GameObject pitcher;
    private Image judgeTriple;
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
			field.GetComponent<BaseCondition> ().BaseStateMachine(3);
			field.GetComponent<Game> ().isHitting = true;
			field.GetComponent<Game> ().ToNextPlayer();
			field.GetComponent<Game>().isBallFlying = false;
			Invoke ("SwitchCamera", 3.0f);
		}
	}

	private void SwitchCamera(){
		field.GetComponent<Game>().isBallCameraMoving = false;
		pitcher.GetComponent<Pitch> ().cloneBall.SetActive (false);
		pitcher.GetComponent<Pitch> ().EnableChooseButton ();
        judgeTriple.GetComponent<Pitch>().judgeTriple.enabled = true;
        //field.GetComponent<Game> ().SetSituation ("Triple");
		if (field.GetComponent<Game>().nowAttack == "visiting") {
			pitcher.GetComponent<Pitch> ().EnableReadyBtn ();
			field.GetComponent<SwitchCamera>().SwitchToHitterCamera();
		} else {
			pitcher.GetComponent<Pitch>().EnableChooseButton();
		}
	}
}
