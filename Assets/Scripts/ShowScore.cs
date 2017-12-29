using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShowScore : MonoBehaviour {
	public Text Top1;
	public Text bottom1;
	public Text Top2;
	public Text bottom2;
	public Text Top3;
	public Text bottom3;
	public Text Top4;
	public Text bottom4;
	public Text Top5;
	public Text bottom5;
	public Text Top6;
	public Text bottom6;
	public Text homeScore;
	public Text visitorScore;
	public Text homeHitNum;
	public Text visitorHitNum;

	private bool isTopInning;
	private int inning;
	private string currentInningScore;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		inning = gameObject.GetComponent<Game> ().inning;
		isTopInning = gameObject.GetComponent<Game> ().isTopInning;
		currentInningScore = gameObject.GetComponent<Game> ().currentInningScore.ToString();

		homeScore.text = gameObject.GetComponent<Game> ().homeScore.ToString ();
		visitorScore.text = gameObject.GetComponent<Game> ().visitorScore.ToString ();
		homeHitNum.text = gameObject.GetComponent<Game> ().homeHitNum.ToString ();
		visitorHitNum.text = gameObject.GetComponent<Game> ().visitorHitNum.ToString ();
	}

	public void Show(){
		if (inning == 1 && isTopInning) {
			Top1.text = currentInningScore;
		}
		if(inning == 1 && !isTopInning){
			bottom1.text = currentInningScore;
		}
		if(inning == 2 && isTopInning){
			Top2.text = currentInningScore;
		}
		if(inning == 2 && !isTopInning){
			bottom2.text = currentInningScore;
		}
		if(inning == 3 && isTopInning){
			Top3.text = currentInningScore;
		}
		if(inning == 3 && !isTopInning){
			bottom3.text = currentInningScore;
		}
		if(inning == 4 && isTopInning){
			Top4.text = currentInningScore;
		}
		if(inning == 4 && !isTopInning){
			bottom4.text = currentInningScore;
		}
		if(inning == 5 && isTopInning){
			Top5.text = currentInningScore;
		}
		if(inning == 5 && !isTopInning){
			bottom5.text = currentInningScore;
		}
		if(inning == 6 && isTopInning){
			Top6.text = currentInningScore;
		}
		if(inning == 6 && !isTopInning){
			bottom6.text = currentInningScore;
		}
	}
}
