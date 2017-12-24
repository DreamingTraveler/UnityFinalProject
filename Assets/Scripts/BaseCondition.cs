using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BaseCondition : MonoBehaviour {
    private GameObject base1;
    private GameObject base2;
    private GameObject base3;

	public Image baseImage1;
	public Image baseImage2;
	public Image baseImage3;

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

	public void BaseStateMachine(int baseNum) {
		switch (baseNum) {
		case 1:
			PushOneBase ();
			break;
		case 2:
			PushTwoBase ();
			break;
		case 3:
			PushThreeBase ();
			break;
		default:
			break;
		}

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
		SetBase (baseConditionNow);
	}

	private void PushTwoBase(){
		if (baseConditionNow == "Empty") {
			baseConditionNow = "Two";
		} else if (baseConditionNow == "One") {
			baseConditionNow = "TwoThree";
		} else if (baseConditionNow == "Two" || baseConditionNow == "Three") {
			GameObject.Find ("Field").GetComponent<Game> ().AddPoint (1);
			baseConditionNow = "Two";
		} else if (baseConditionNow == "OneThree" || baseConditionNow == "OneTwo") {
			GameObject.Find ("Field").GetComponent<Game> ().AddPoint (1);
			baseConditionNow = "TwoThree";
		} else if (baseConditionNow == "TwoThree") {
			GameObject.Find ("Field").GetComponent<Game> ().AddPoint (2);
			baseConditionNow = "Two";
		} else if (baseConditionNow == "Full") {
			GameObject.Find ("Field").GetComponent<Game> ().AddPoint (2);
			baseConditionNow = "TwoThree";
		}
		SetBase (baseConditionNow);
	}

	private void PushThreeBase(){
		if (baseConditionNow == "One" || baseConditionNow == "Two" || baseConditionNow == "Three") {
			GameObject.Find ("Field").GetComponent<Game> ().AddPoint (1);
		} else if (baseConditionNow == "OneThree" || baseConditionNow == "OneTwo" || baseConditionNow == "TwoThree") {
			GameObject.Find ("Field").GetComponent<Game> ().AddPoint (2);
		} else if (baseConditionNow == "Full") {
			GameObject.Find ("Field").GetComponent<Game> ().AddPoint (3);
		}
		baseConditionNow = "Three";
		SetBase (baseConditionNow);
	}

	public void SetBase(string condition) {
        switch (condition){
		case "Empty":
			baseConditionNow = "Empty";
			playerOnBaseNum = 0;
			base1.GetComponent<MeshRenderer> ().material.color = Color.white;
			base2.GetComponent<MeshRenderer> ().material.color = Color.white;
			base3.GetComponent<MeshRenderer> ().material.color = Color.white;
			baseImage1.GetComponent<Image> ().color = Color.white;
			baseImage2.GetComponent<Image> ().color = Color.white;
			baseImage3.GetComponent<Image> ().color = Color.white;
            break;
		case "One":
			base1.GetComponent<MeshRenderer> ().material.color = Color.red;
			base2.GetComponent<MeshRenderer> ().material.color = Color.white;
			base3.GetComponent<MeshRenderer> ().material.color = Color.white;
			baseImage1.GetComponent<Image> ().color = Color.red;
			baseImage2.GetComponent<Image> ().color = Color.white;
			baseImage3.GetComponent<Image> ().color = Color.white;
			playerOnBaseNum = 1;
            break;
        case "Two":
            base1.GetComponent<MeshRenderer>().material.color = Color.white;
            base2.GetComponent<MeshRenderer>().material.color = Color.red;
            base3.GetComponent<MeshRenderer>().material.color = Color.white;
			baseImage1.GetComponent<Image> ().color = Color.white;
			baseImage2.GetComponent<Image> ().color = Color.red;
			baseImage3.GetComponent<Image> ().color = Color.white;
		    playerOnBaseNum = 1;
            break;
        case "Three":
            base1.GetComponent<MeshRenderer>().material.color = Color.white;
            base2.GetComponent<MeshRenderer>().material.color = Color.white;
            base3.GetComponent<MeshRenderer>().material.color = Color.red;
			baseImage1.GetComponent<Image> ().color = Color.white;
			baseImage2.GetComponent<Image> ().color = Color.white;
			baseImage3.GetComponent<Image> ().color = Color.red;
		    playerOnBaseNum = 1;
            break;
        case "OneTwo":
			base1.GetComponent<MeshRenderer>().material.color = Color.red;
			base2.GetComponent<MeshRenderer>().material.color = Color.red;
			base3.GetComponent<MeshRenderer>().material.color = Color.white;
			baseImage1.GetComponent<Image> ().color = Color.red;
			baseImage2.GetComponent<Image> ().color = Color.red;
			baseImage3.GetComponent<Image> ().color = Color.white;
			playerOnBaseNum = 2;
			break;
		case "OneThree":
			base1.GetComponent<MeshRenderer>().material.color = Color.red;
			base2.GetComponent<MeshRenderer>().material.color = Color.white;
			base3.GetComponent<MeshRenderer>().material.color = Color.red;
			baseImage1.GetComponent<Image> ().color = Color.red;
			baseImage2.GetComponent<Image> ().color = Color.white;
			baseImage3.GetComponent<Image> ().color = Color.red;
			playerOnBaseNum = 2;
			break;    
		case "TwoThree":
			base1.GetComponent<MeshRenderer>().material.color = Color.white;
			base2.GetComponent<MeshRenderer>().material.color = Color.red;
			base3.GetComponent<MeshRenderer>().material.color = Color.red;
			baseImage1.GetComponent<Image> ().color = Color.white;
			baseImage2.GetComponent<Image> ().color = Color.red;
			baseImage3.GetComponent<Image> ().color = Color.red;
			playerOnBaseNum = 2;
			break;
		case "Full":
			base1.GetComponent<MeshRenderer>().material.color = Color.red;
			base2.GetComponent<MeshRenderer>().material.color = Color.red;
			base3.GetComponent<MeshRenderer>().material.color = Color.red;
			baseImage1.GetComponent<Image> ().color = Color.red;
			baseImage2.GetComponent<Image> ().color = Color.red;
			baseImage3.GetComponent<Image> ().color = Color.red;
			playerOnBaseNum = 3;
			break;   
        }
    }
}
