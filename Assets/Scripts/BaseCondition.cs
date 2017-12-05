using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseCondition : MonoBehaviour {
    private GameObject base1;
    private GameObject base2;
    private GameObject base3;

    public string baseConditionNow = "Empty";
	public int playerOnBaseNum = 0;
    // Use this for initialization
    void Start() {
        base1 = GameObject.Find("Base1");
        base2 = GameObject.Find("Base2");
        base3 = GameObject.Find("Base3");
    }

    // Update is called once per frame
    void Update() {

    }

    public void BaseStateMachine() {
		PushOneBase ();
    }

	private void PushOneBase(){
		if (baseConditionNow == "Empty") {
			baseConditionNow = "One";
		} else if (baseConditionNow == "One" || baseConditionNow == "Two") {
			baseConditionNow = "OneTwo";
		} else if (baseConditionNow == "Three") {
			baseConditionNow = "OneThree";
		} else if (baseConditionNow == "OneTwo" || baseConditionNow == "OneThree" || baseConditionNow == "TwoThree") {
			baseConditionNow = "Full";
		} else if (baseConditionNow == "Full") {
			GameObject.Find ("Field").GetComponent<Game> ().AddPoint (1);
		}
		SetBase(baseConditionNow);
	}

	public void SetBase(string condition) {
        switch (condition){
			case "Empty":
				playerOnBaseNum = 0;
				base1.GetComponent<MeshRenderer> ().material.color = Color.white;
				base2.GetComponent<MeshRenderer> ().material.color = Color.white;
				base3.GetComponent<MeshRenderer> ().material.color = Color.white;
                break;
			case "One":
				base1.GetComponent<MeshRenderer> ().material.color = Color.red;
				base2.GetComponent<MeshRenderer> ().material.color = Color.white;
				base3.GetComponent<MeshRenderer> ().material.color = Color.white;
				playerOnBaseNum = 1;
                break;
            case "Two":
                base1.GetComponent<MeshRenderer>().material.color = Color.white;
                base2.GetComponent<MeshRenderer>().material.color = Color.red;
                base3.GetComponent<MeshRenderer>().material.color = Color.white;
				playerOnBaseNum = 1;
                break;
            case "Three":
                base1.GetComponent<MeshRenderer>().material.color = Color.white;
                base2.GetComponent<MeshRenderer>().material.color = Color.white;
                base3.GetComponent<MeshRenderer>().material.color = Color.red;
				playerOnBaseNum = 1;
                break;
            case "OneTwo":
                base1.GetComponent<MeshRenderer>().material.color = Color.red;
                base2.GetComponent<MeshRenderer>().material.color = Color.red;
                base3.GetComponent<MeshRenderer>().material.color = Color.white;
				playerOnBaseNum = 2;
                break;
            case "OneThree":
                base1.GetComponent<MeshRenderer>().material.color = Color.red;
                base2.GetComponent<MeshRenderer>().material.color = Color.white;
                base3.GetComponent<MeshRenderer>().material.color = Color.red;
				playerOnBaseNum = 2;
                break;
            case "TwoThree":
                base1.GetComponent<MeshRenderer>().material.color = Color.white;
                base2.GetComponent<MeshRenderer>().material.color = Color.red;
                base3.GetComponent<MeshRenderer>().material.color = Color.red;
				playerOnBaseNum = 2;
                break;
            case "Full":
                base1.GetComponent<MeshRenderer>().material.color = Color.red;
                base2.GetComponent<MeshRenderer>().material.color = Color.red;
                base3.GetComponent<MeshRenderer>().material.color = Color.red;
				playerOnBaseNum = 3;
                break;
        }
    }
}
