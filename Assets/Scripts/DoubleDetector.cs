using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DoubleDetector : MonoBehaviour {
	public GameObject field;
	public GameObject pitcher;
    private GameObject judgeDouble;
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
			field.GetComponent<Game> ().ShowImage ("Double");
			Invoke ("SwitchCamera", 3.0f);
		}
	}

	private void SwitchCamera(){
		field.GetComponent<Game>().isBallCameraMoving = false;
		pitcher.GetComponent<Pitch> ().cloneBall.SetActive (false);
        judgeDouble.GetComponent<Pitch>().judgeDouble.enabled = true;
        //field.GetComponent<Game> ().SetSituation ("Double");
		field.GetComponent<Game>().isHitting = false;
		if (field.GetComponent<Game>().nowAttack == "visitor") {
			pitcher.GetComponent<Pitch> ().EnableReadyBtn ();
			field.GetComponent<SwitchCamera>().SwitchToHitterCamera();
		} else {
			pitcher.GetComponent<Pitch>().EnableChooseButton();
		}
	}
}
