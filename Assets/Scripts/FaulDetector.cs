using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaulDetector : MonoBehaviour {
	public GameObject field;
	public GameObject pitcher;
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
        field.GetComponent<Game> ().SetSituation ("Faul");
		field.GetComponent<Game>().isBallFlying = false;
		col.gameObject.SetActive (false);
		pitcher.GetComponent<Pitch> ().EnableChooseButton ();
	}
}
