using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitBall : MonoBehaviour {
	public Animator animator;

	GameObject ball;
	GameObject hitting_point;
	GameObject pitcher;
	Rigidbody rig;

	public GameObject strikeDetector;
	public GameObject ballDetector;
	public float hitting_force;
	public float randomY;
	public bool isSwing = false;
	public bool canHitBall = false;
	public Dictionary<string,string> forceTable = new Dictionary<string,string> ();

	private GameObject field;
	// Use this for initialization
	void Start () {
		animator = GetComponent<Animator> ();
		field = GameObject.Find ("Field");
		pitcher = GameObject.FindGameObjectWithTag ("Pitcher");
		hitting_point = GameObject.Find ("Hitting_Point");
		CreateForceTable ();
	}
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown("space") && field.GetComponent<Game>().nowAttack == "visitor"){
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
		float notHitProb = Random.Range (1, 10);

		isSwing = true;
		print (ballHorPos.ToString("0.0"));
		MatchForce (ballHorPos.ToString("0.0"));
		if (CanHit (ball) && notHitProb > 2) {
			strikeDetector.GetComponent<DetectStrike>().ChangeImagePosition ();
			ballDetector.GetComponent<DetectBall>().ChangeImagePosition ();
			randomY = Random.Range (-200f, 720f);
			ball.GetComponent<Rigidbody> ().velocity = (new Vector3 (hitting_point.x, randomY, hitting_point.z)).normalized * hitting_force;
			field.GetComponent<Game> ().SetBall(ball);
            field.GetComponent<SwitchCamera>().SwitchToBallCamera();
            field.GetComponent<FollowBall>().SetBall(ball);
		} else {
			pitcher.GetComponent<Pitch> ().strike++;
			if (pitcher.GetComponent<Pitch> ().strike < 3) {
				field.GetComponent<Game> ().ShowImage ("Strike");
			}
		}
	}
}

//leftbottom : 207.52  12.4  200.34
//rightbottom : 200.2  12.4  207.6
//righttop : 207.2  25.4  215
//lefttop : 214.6 25.4 207.6