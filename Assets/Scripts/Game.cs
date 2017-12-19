﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Game : MonoBehaviour {
    public int homeScore;
    public int visitingScore;
    public int inning = 1;
	public bool isTopInning = true;
	public string nowAttack = "visiting";
	public bool isHitting = false;
	public bool isBallFlying = false;

	private GameObject HomeRunWall;
	private GameObject pitcher;
	private GameObject ball;
    private Text pointText;
	private Text inningText;
	// Use this for initialization
	void Start () {
		pitcher = GameObject.FindGameObjectWithTag ("Pitcher");
        pointText = GameObject.Find("Point").GetComponent<Text>();
		inningText = GameObject.Find ("Inning").GetComponent<Text> ();
		HomeRunWall = GameObject.Find ("HomerunWall");
	}
	
	// Update is called once per frame
	void Update () {
		SetText ();
		if (pitcher.GetComponent<Pitch> ().outNum == 3) {
			ToNextHalfInning ();
		}

		if (isBallFlying) {
			JudgeOutBall ();
		}

		StrikeoutAndFourBall ();
	}

	public void SetBall(GameObject cloneBall){
		isBallFlying = true;
		ball = cloneBall;
	}

	private void SetText(){
		pointText.text = homeScore + " - " + visitingScore;
		string topBottomSign;
		if (isTopInning) {
			topBottomSign = "△";
		} else {
			topBottomSign = "▽";
		}
		inningText.text = inning + " " + topBottomSign;
	}

	private void ToNextHalfInning(){
		pitcher.GetComponent<Pitch> ().outNum = 0;
		gameObject.GetComponent<BaseCondition>().SetBase("Empty");
		if (isTopInning) {//Top -> Bottom
			nowAttack = "home";
			isTopInning = false;
		} else {//Bottom -> Top (Change Inning)
			nowAttack = "visiting";
			isTopInning = true;
			inning++;
		}
	}

	public void AddPoint(int point){
		if (nowAttack == "home") {
			homeScore += point;
		} else if (nowAttack == "visiting") {
			visitingScore += point;
		}
	}

	public void StrikeoutAndFourBall(){
		int strike = pitcher.GetComponent<Pitch> ().strike;
		int badBall = pitcher.GetComponent<Pitch> ().badBall;
		if (strike == 3) {//strikeout!
			pitcher.GetComponent<Pitch>().outNum++;
            ToNextPlayer();
		} else if (badBall == 4) {
            ToNextPlayer();
			gameObject.GetComponent<BaseCondition> ().BaseStateMachine (1);
		}
	}

    public void ToNextPlayer() {
        pitcher.GetComponent<Pitch>().strike = 0;
        pitcher.GetComponent<Pitch>().badBall = 0;
    }

	public void JudgeOutBall(){
		if (ball.transform.position.y <= 1.0f) {//The ball falls to the field
			isBallFlying = false;
			if ((ball.transform.position.x < 200f || ball.transform.position.z < 200f)) {//faul
				if(pitcher.GetComponent<Pitch> ().strike < 2){
					pitcher.GetComponent<Pitch> ().strike++;
				}
			} else if (!isHitting && (ball.transform.position.x > 200f && ball.transform.position.z > 200f)) {
				pitcher.GetComponent<Pitch> ().outNum++;
				ToNextPlayer();
			}
			isHitting = false;
			pitcher.GetComponent<Pitch> ().cloneBall.SetActive (false);
			pitcher.GetComponent<Pitch> ().EnableChooseButton ();
		}
	}
}
