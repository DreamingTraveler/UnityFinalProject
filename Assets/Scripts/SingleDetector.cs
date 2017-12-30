using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SingleDetector : MonoBehaviour {
	public GameObject field;
	public GameObject pitcher;
    private Image judgeSingle;
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
			field.GetComponent<BaseCondition> ().BaseStateMachine(1);
			field.GetComponent<Game> ().isHitting = true;
			field.GetComponent<Game> ().ToNextPlayer();
			field.GetComponent<Game>().isBallFlying = false;
			field.GetComponent<Game>().AddHitNum();
			field.GetComponent<Game> ().ShowImage ("Single");
			Invoke ("SwitchCamera", 3.0f);
		}
	}

	private void SwitchCamera(){
		field.GetComponent<Game>().isBallCameraMoving = false;
		pitcher.GetComponent<Pitch> ().cloneBall.SetActive (false);
        judgeSingle.GetComponent<Pitch>().judgeSingle.enabled = true;
        //field.GetComponent<Game> ().SetSituation ("Single");
		field.GetComponent<Game>().isHitting = false;
		if (field.GetComponent<Game>().nowAttack == "visitor") {
			pitcher.GetComponent<Pitch> ().EnableReadyBtn ();
			field.GetComponent<SwitchCamera>().SwitchToHitterCamera();
		} else {
			pitcher.GetComponent<Pitch>().EnableChooseButton();
		}
	}
}
