using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FaulDetector : MonoBehaviour {
	public GameObject field;
	public GameObject pitcher;
    private Image judgeFaul;
	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnCollisionEnter(Collision col){
		if (pitcher.GetComponent<Pitch>().strike < 2) {//out ball
			pitcher.GetComponent<Pitch>().strike++;
		}
        judgeFaul.GetComponent<Pitch>().judgeFoulBall.enabled = true;
        //field.GetComponent<Game> ().SetSituation ("Faul");
		field.GetComponent<Game>().isBallFlying = false;
		col.gameObject.SetActive (false);
		if (field.GetComponent<Game>().nowAttack == "visitor") {
			pitcher.GetComponent<Pitch> ().EnableReadyBtn ();
			field.GetComponent<SwitchCamera>().SwitchToHitterCamera();
		} else {
			pitcher.GetComponent<Pitch>().EnableChooseButton();
		}
	}
}
