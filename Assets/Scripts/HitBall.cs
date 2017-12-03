using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitBall : MonoBehaviour {
	Animator animator;

	GameObject ball;
	GameObject hitting_point;
	GameObject pitcher;
	Rigidbody rig;

	public float hitting_force;
	public bool isSwing = false;
	public bool canHitBall = false;
	public Dictionary<string,string> forceTable = new Dictionary<string,string> ();
	//Vector3 pitchPos = GameObject.Find ("Pitching_Point").transform.position;
	// Use this for initialization
	void Start () {
		animator = GetComponent<Animator> ();
		//ball = GameObject.FindGameObjectWithTag("Ball");
		pitcher = GameObject.FindGameObjectWithTag ("Pitcher");
		hitting_point = GameObject.Find ("Hitting_Point");
		CreateForceTable ();
	}
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown("space")){
			animator.SetTrigger ("isHit");
		}
	}

	private void CreateForceTable(){
		float ballForce = 20f;
		for (float ballPos = 346f; ballPos <= 448.1f; ballPos += 0.1f) {
			forceTable.Add (ballPos.ToString("0.0"),ballForce.ToString("0.00"));
			if (ballPos < 397f) {
				ballForce += 0.15f;
			} else {
				ballForce -= 0.15f;
			}
		}
		foreach (KeyValuePair<string,string>kvp in forceTable) {
			print (kvp.Key + " | " + kvp.Value); 
		}
	}

	public bool CanHit(GameObject ball){
		Vector3 ballPos = ball.transform.position;
		if(ballPos.x >= 171.0f && ballPos.x <= 220.0f && ballPos.y >= 11.0f && ballPos.y <= 27.0f && ballPos.z >= 176.0f && ballPos.z <= 228.0f){
			return true;
		}
		return false;
	}	

	private void MatchForce(string ballPos){
		if (forceTable.ContainsKey (ballPos)) {
			hitting_force = float.Parse(forceTable [ballPos]);
		}
	}
	public void Swing(GameObject ball){
		Vector3 pitchPos = GameObject.Find ("Pitching_Point").transform.position;
		Vector3 hitting_point = GameObject.Find ("Hitting_Point").transform.position;
		float ballHorPos = ball.transform.position.x + ball.transform.position.z;
		print (ballHorPos.ToString("0.0"));
		MatchForce (ballHorPos.ToString("0.0"));
		if (CanHit (ball)) {
			ball.GetComponent<Rigidbody> ().velocity = (new Vector3 (hitting_point.x, Input.mousePosition.y, hitting_point.z)).normalized * hitting_force;
		} else {
			pitcher.GetComponent<Pitch> ().strike++;
		}
	}
}

//leftbottom : 207.52  12.4  200.34
//rightbottom : 200.2  12.4  207.6
//righttop : 207.2  25.4  215
//lefttop : 214.6 25.4 207.6