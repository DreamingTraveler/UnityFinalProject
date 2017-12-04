using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Pitch : MonoBehaviour {
	Animator pitcherAnimator;
	Animator ballAnimator;
	public GameObject ball;
	public float speed;
	public float w, h;
	public bool isPitching = false;
	public Button slider_btn;
	public int ballMode = 0;
	public float hittingPointMovingSpeed;
	public Vector3 targetPos;
	public bool canChooseBall = true;
	public int strike;
	public int badBall;

	private Vector3 pitchPos;
	private Vector3 tempPos;

	private GameObject cloneBall;
	private GameObject hitter;
	private GameObject hittingPoint;
	private GameObject targetPoint;
	private GameObject cursor;
	private MeshRenderer targetMesh;
	private Button confirmBallPos;
	private Text StrikeBall;
	// Use this for initialization
	void Start () {
		pitcherAnimator = GameObject.FindGameObjectWithTag("Pitcher").GetComponent<Animator> ();
		pitcherAnimator.Play("Pitcher_Idle");
		hitter = GameObject.FindGameObjectWithTag ("Hitter");
		hittingPoint = GameObject.Find ("Hitting_Point");
		targetPoint = GameObject.Find ("Pitching_Target");
		targetPos = targetPoint.transform.position;
		targetMesh = targetPoint.GetComponent<MeshRenderer> ();
		cursor = GameObject.FindGameObjectWithTag ("Cursor");
		confirmBallPos = GameObject.Find ("Confirm").GetComponent<Button>();
		StrikeBall = GameObject.Find ("StrikeBall").GetComponent<Text> ();
	}
		
	//judgeleftbottom : 262.5  0  154.5
	//judgelefttop : 262.5  39.6  154.5
	//judgerighttop : 197.8  39.6  219.2
	//judgerightbottom : 197.8  0  219.2
	// Update is called once per frame
	void Update () {
		if (isPitching) {
			CallHitter (cloneBall);
			Vector3 ballPos = cloneBall.transform.position;
			for(int i = 0; i < 50000; i++){
				if (ballPos.x + ballPos.z >= 405f && ballPos.x + ballPos.z <= 415f) {
					RecordBallPos ();
				}
			}
			StopBall (cloneBall);
		}
			
		if (canChooseBall) {
			ChooseBallPosition ();
			targetPoint.transform.position = targetPos;
		} else {
			targetMesh.enabled = false;
			confirmBallPos.gameObject.SetActive (false);
		}
		StrikeBall.text = badBall + " - " + strike;
	}

	public void ChooseBallPosition(){
		targetMesh.enabled = true;
		confirmBallPos.gameObject.SetActive (true);
		if (Input.GetKey(KeyCode.UpArrow)) {
			targetPos.y += 0.1f;
		}

		if (Input.GetKey(KeyCode.DownArrow)) {
			targetPos.y -= 0.1f;
		}

		if (Input.GetKey(KeyCode.RightArrow)) {
			targetPos.x -= 0.1f;
			targetPos.z += 0.1f;
		}

		if (Input.GetKey(KeyCode.LeftArrow)) {
			targetPos.x += 0.1f;
			targetPos.z -= 0.1f;
		}
	}

	private void RecordBallPos(){
		tempPos = cloneBall.transform.position;
	}

	private void StopBall(GameObject cloneBall){
		if (cloneBall.transform.position.x + cloneBall.transform.position.z < 336f) {
			cloneBall.GetComponent<Rigidbody> ().velocity = new Vector3(0f,0f,0f) * 0f;
			JudgeBall ();
			print (tempPos);
			isPitching = false;
		}
	}

	public void ChooseBallType(){
		canChooseBall = true;
		hitter.GetComponent<HitBall> ().hitting_force = 0;
	}

	public void CallAnimate(){
		canChooseBall = false;

		Invoke ("PitchAnimate", 2.0f);
	}

	private void PitchAnimate(){
		pitcherAnimator.SetTrigger ("isPitch");

		hittingPoint.transform.position = new Vector3 (-260f, 19.3f, 726f);

		Invoke ("PitchBall", 1.0f);
	}

	//leftbottom : 213.6  13.3  204.6
	//rightbottom : 213.6  13.3 204.6
	//righttop : 204.6  24.9 213.6
	//lefttop : 204.6 24.9 213.6

	//Bleftbottom : 211.6  11  184.6
	//Brightbottom : 184.6 11 211.6
	//Brighttop : 184.6 24.9 211.6
	//Blefttop : 211.6 24.9 184.6
	public void JudgeBall(){
		Vector3 ballPos = tempPos;
		if (ballPos.x >= 198.5 && ballPos.x <= 210.3f && ballPos.y >= 12.5f && ballPos.y <= 25f && ballPos.z >= 199.5f && ballPos.z <= 211f) {
			strike++;
		} else
			badBall++;
		//isPitching = false;
	}

	public void PitchBall(){
		Vector3 pitchPos = GameObject.Find ("Pitching_Point").transform.position;																																										

		cloneBall = Instantiate (ball) as GameObject;
		cloneBall.name = "CloneBall";
		cloneBall.transform.position = pitchPos;

		cloneBall.GetComponent<Rigidbody> ().velocity = (targetPos - pitchPos).normalized * speed;
		if (ballMode != 0) {
			gameObject.GetComponent<BreakBall> ().SetBreakBall (cloneBall, ballMode);
		}

		isPitching = true;
	}

	public void SetModeAsFourSeam(){
		ballMode = 0;
		hittingPointMovingSpeed = 8000f;
		speed = 350f;
	}

	public void SetModeAsSlider(){
		ballMode = 1;
		hittingPointMovingSpeed = 9000f;
		speed = 270f;
	}

	public void SetModeAsCutter(){
		ballMode = 2;
		hittingPointMovingSpeed = 8000f;
		speed = 320f;
	}

	public void SetModeAsFork(){
		ballMode = 3;
		hittingPointMovingSpeed = 9000f;
		speed = 270f;
	}

	private void CallHitter(GameObject cloneBall){
		MoveHittingPoint (cloneBall);
		if (Input.GetKeyDown ("space")) {
            hitter.GetComponent<HitBall>().Swing(cloneBall);
        }
	}
		
	private void MoveHittingPoint(GameObject cloneBall){
		if (/*cloneBall.transform.position.x + cloneBall.transform.position.z >= 347f &&
			cloneBall.transform.position.x + cloneBall.transform.position.z <= 448f*/
			hitter.GetComponent<HitBall>().CanHit(cloneBall)) {
			//hitting_point.GetComponent<Rigidbody> ().velocity = hitting_point_end.normalized * 100.0f;
			hittingPoint.transform.Translate(Vector3.forward * Time.deltaTime * hittingPointMovingSpeed);
		}
	}
}
