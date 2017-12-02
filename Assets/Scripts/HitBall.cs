using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitBall : MonoBehaviour {
	Animator animator;

	GameObject ball;
	GameObject hitting_point;
	GameObject pitcher;
	Rigidbody rig;
	public bool isSwing = false;
	public bool canHitBall = false;
	//Vector3 pitchPos = GameObject.Find ("Pitching_Point").transform.position;
	// Use this for initialization
	void Start () {
		animator = GetComponent<Animator> ();
		//ball = GameObject.FindGameObjectWithTag("Ball");
		pitcher = GameObject.FindGameObjectWithTag ("Pitcher");
		hitting_point = GameObject.Find ("Hitting_Point");
	}
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown("space")){
			animator.SetTrigger ("isHit");
		}
	}
		
	public bool CanHit(GameObject ball){
		Vector3 ballPos = ball.transform.position;
		if(ballPos.x >= 171.0f && ballPos.x <= 220.0f && ballPos.y >= 11.0f && ballPos.y <= 27.0f && ballPos.z >= 176.0f && ballPos.z <= 228.0f){
			return true;
		}
		return false;
	}	
	public void Swing(GameObject ball){
		Vector3 pitchPos = GameObject.Find ("Pitching_Point").transform.position;
		Vector3 hitting_point = GameObject.Find ("Hitting_Point").transform.position;
		print (ball.transform.position);
		if (CanHit (ball)) {
			ball.GetComponent<Rigidbody> ().velocity = (new Vector3 (hitting_point.x, Input.mousePosition.y, hitting_point.z)).normalized * 88.2f;
		} else {
			pitcher.GetComponent<Pitch> ().strike++;
		}
	}
}

//leftbottom : 207.52  12.4  200.34
//rightbottom : 200.2  12.4  207.6
//righttop : 207.2  25.4  215
//lefttop : 214.6 25.4 207.6