using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Pitch : MonoBehaviour {
	Animator pitcherAnimator;
	Animator ballAnimator;
	public GameObject ball;
    //public GameObject strikeZone;
	public GameObject cloneBall;
	public GameObject ballPositionImage;
	public GameObject strikeZone;
	public float speed;
	public float w, h;
	public bool isPitching = false;
	public bool isStrike = false;
	public Button slider_btn;
	public int ballMode = 0;
	public float hittingPointMovingSpeed;
	public Vector3 targetPos;
	public bool canChooseBall = true;
	public int strike;
	public int badBall;

	public Camera pitcherCamera;
	public Camera hitterCamera;

	private float probHit;
	private Vector3 pitchPos;
	private Vector3 tempPos;

    private GameObject field;
	private GameObject hitter;
	private GameObject hittingPoint;
	private GameObject targetPoint;
	private GameObject cursor;

	private MeshRenderer targetMesh;
	private RectTransform ballSituation;

	private Button confirmBallPos;
	private Button fourSeamBtn;
	private Button sliderBtn;
	private Button cutterBtn;
	private Button forkballBtn;
	private Button readyToHitBtn;

	private Text strikeBall;
	private Text speedText;
    // Use this for initialization
    void Start () {
		pitcherAnimator = GameObject.FindGameObjectWithTag("Pitcher").GetComponent<Animator> ();
		pitcherAnimator.Play("Pitcher_Idle");
		ballSituation = GameObject.Find ("BallSituation").GetComponent<RectTransform> ();
        field = GameObject.Find("Field");
        hitter = GameObject.FindGameObjectWithTag ("Hitter");
		hittingPoint = GameObject.Find ("Hitting_Point");
		targetPoint = GameObject.Find ("Pitching_Target");
		targetPos = targetPoint.transform.position;
		targetMesh = targetPoint.GetComponent<MeshRenderer> ();
		cursor = GameObject.FindGameObjectWithTag ("Cursor");

		confirmBallPos = GameObject.Find ("Confirm").GetComponent<Button>();
		fourSeamBtn = GameObject.Find ("FourSeam").GetComponent<Button> ();
		sliderBtn = GameObject.Find ("Slider").GetComponent<Button> ();
		cutterBtn = GameObject.Find ("Cutter").GetComponent<Button> ();
		forkballBtn = GameObject.Find ("Forkball").GetComponent<Button> ();
		readyToHitBtn = GameObject.Find ("Ready").GetComponent<Button> ();

		strikeBall = GameObject.Find ("StrikeBall").GetComponent<Text> ();
		speedText = GameObject.Find ("Speed").GetComponent<Text> ();
    }
		
	// Update is called once per frame
	void Update () {
		if (isPitching) {
			CallHitter (cloneBall);
            Vector3 ballPos = cloneBall.transform.position;
            for (int i = 0; i < 1000000; i++){
                if (ballPos.x + ballPos.z >= 404.5f && ballPos.x + ballPos.z <= 415.5f) {
                    RecordBallPos();
                }
            }
            StopBall(cloneBall);
		} 
		if (field.GetComponent<Game> ().nowAttack == "visitor") {
			ballSituation.localPosition = new Vector3 (93, 106, 0);
			strikeZone.GetComponent<MeshRenderer> ().enabled = false;
			DisableChooseButton ();
		} else {
			ballSituation.localPosition = new Vector3 (147, 259, 0);
			strikeZone.GetComponent<MeshRenderer> ().enabled = true;
			readyToHitBtn.gameObject.SetActive (false);
		}
			
		if (canChooseBall) {
            ChooseBallPosition ();
			targetPoint.transform.position = targetPos;
		} else {
			targetMesh.enabled = false;
			confirmBallPos.gameObject.SetActive (false);
		}

		if (field.GetComponent<Game> ().nowAttack == "home" && isPitching) {
			if (hitter.GetComponent<HitBall>().CanHit(cloneBall)) {
				StartCoroutine (AutoHitTime(cloneBall));
			}
		}
		strikeBall.text = badBall + " - " + strike;

	}

	IEnumerator AutoHitTime(GameObject cloneBall){
		float randomNum = Random.Range (0.01f, 0.09f);

		yield return new WaitForSeconds (randomNum);
		if (hitter.GetComponent<HitBall> ().isSwing == false) {
			AutoHit ();
		}
	}

	private void AutoHit(){
		if(!(targetPos.x >= 198.5f && targetPos.x <= 209.3f && targetPos.y >= 12.5f && targetPos.y <= 25f && 
			targetPos.z >= 199.5f && targetPos.z <= 210.6f) && probHit > 10f){//ball
			return;
		}
		if ((strike == 0 && badBall == 0) && (probHit > 70f) ||
			(strike < 2 && badBall < 2) && (probHit > 50f) ||
			(strike < badBall && probHit < 30f) ||
			(strike == 2 && badBall < 3) && (probHit > 10f)) {
			AutoSwing ();
		}
	}

	private void AutoSwing(){
		hitter.GetComponent<HitBall>().animator.SetTrigger ("isHit");
		hitter.GetComponent<HitBall>().Swing(cloneBall);
	}

	public void AutoPitch(){
		float randomNumForBallType = Random.Range (1f, 100f);

		if (randomNumForBallType < 50f) {
			targetPos = new Vector3(Random.Range(195f, 215f), Random.Range(13f,27f), Random.Range(195f,215f));
			SetModeAsFourSeam ();
		} else if (randomNumForBallType >= 50f && randomNumForBallType < 67f) {
			targetPos = new Vector3(Random.Range(197f, 207f), Random.Range(16f,27f), Random.Range(203f,213f));
			SetModeAsSlider ();
		} else if (randomNumForBallType >= 67f && randomNumForBallType < 84f) {
			targetPos = new Vector3(Random.Range(197f, 207f), Random.Range(16f,27f), Random.Range(203f,213f));
			SetModeAsCutter ();
		} else {
			targetPos = new Vector3(Random.Range(197f, 207f), Random.Range(19f,28f), Random.Range(195f,213f));
			SetModeAsFork ();
		}
		readyToHitBtn.gameObject.SetActive (false);
		CallAnimate ();
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
			SetCamera ();
			EnableReadyBtn ();
			Destroy (cloneBall);
		}
	}

	public void EnableReadyBtn(){
		readyToHitBtn.gameObject.SetActive (true);
	}

	private void SetCamera(){
		if(field.GetComponent<Game>().nowAttack == "visitor"){
			field.GetComponent<SwitchCamera>().SwitchToHitterCamera();
		}else{
			EnableChooseButton ();
		}
	}

	public void ChooseBallType(){
		canChooseBall = true;
		targetPos = new Vector3 (202.88f, 20.3f, 205f);
	}

	public void CallAnimate(){
		canChooseBall = false;
		isStrike = false;
		DisableChooseButton ();
		hitter.GetComponent<HitBall> ().hitting_force = 0;
		hitter.GetComponent<HitBall> ().isSwing = false;
        //field.GetComponent<SwitchCamera>().SwitchToHitterCamera()

		probHit = Random.Range (1f, 100f);
		print ("ProbHit:" + probHit);
        //strikeZone.SetActive(false);
		Invoke ("PitchAnimate", 2.0f);
	}

	public void EnableChooseButton(){
		fourSeamBtn.gameObject.SetActive (true);
		sliderBtn.gameObject.SetActive (true);
		cutterBtn.gameObject.SetActive (true);
		forkballBtn.gameObject.SetActive (true);
        //strikeZone.SetActive(true);
		field.GetComponent<Game>().isHitting = false;
        field.GetComponent<SwitchCamera>().SwitchToPitcherCamera();
        //Invoke("SetSituationClear", 1.0f);
    }

	public void DisableChooseButton(){
		fourSeamBtn.gameObject.SetActive (false);
		sliderBtn.gameObject.SetActive (false);
		cutterBtn.gameObject.SetActive (false);
		forkballBtn.gameObject.SetActive (false);
	}

	private void PitchAnimate(){
		pitcherAnimator.SetTrigger ("isPitch");

		hittingPoint.transform.position = new Vector3 (-394f, 19.3f, 868f);
		Invoke ("PitchBall", 1.0f);
	}

	public void JudgeBall(){
		Vector3 ballPos = tempPos;
		/*
		if (field.GetComponent<Game> ().nowAttack == "visitor") {
			ballPositionImage.transform.position = hitterCamera.WorldToScreenPoint(ballPos);
		} else {
			ballPositionImage.transform.position = pitcherCamera.WorldToScreenPoint(ballPos);
		}*/
			
		if (hitter.GetComponent<HitBall> ().isSwing == false) {
			if (isStrike) {
				strike++;
				if (strike < 3) {
					field.GetComponent<Game> ().ShowImage ("Strike");
				}
			} else {
				badBall++;
				if (badBall < 4) {
					field.GetComponent<Game> ().ShowImage ("Ball");
				}
			}
		}
		//Invoke ("ChangeImagePosition", 1.5f);
	}

	private void ChangeImagePosition(){
		ballPositionImage.transform.position = new Vector3 (-10 ,-10, -10);
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
		speedText.text = ((speed / 350)*105).ToString("0.0") + "mph";
		isPitching = true;
	}

	public void SetModeAsFourSeam(){
		ballMode = 0;
		hittingPointMovingSpeed = 9000f;
		speed = Random.Range (320f,350f);
	}

	public void SetModeAsSlider(){
		ballMode = 1;
		hittingPointMovingSpeed = 12000f;
		speed = Random.Range (260f,280f);
	}

	public void SetModeAsCutter(){
		ballMode = 2;
		hittingPointMovingSpeed = 10200f;
		speed = Random.Range (300f,320f);
	}

	public void SetModeAsFork(){
		ballMode = 3;
		hittingPointMovingSpeed = 12000f;
		speed = Random.Range (260f,280f);
	}
    
	private void CallHitter(GameObject cloneBall){
		MoveHittingPoint (cloneBall);
		if (Input.GetKeyDown ("space") && hitter.GetComponent<HitBall>().isSwing == false &&
			field.GetComponent<Game>().nowAttack == "visitor") {
            if(cloneBall!=null)
            hitter.GetComponent<HitBall>().Swing(cloneBall);
        }
	}
		
	private void MoveHittingPoint(GameObject cloneBall){
        if (!cloneBall) return;
		if (hitter.GetComponent<HitBall>().CanHit(cloneBall)) {
			hittingPoint.transform.Translate(Vector3.forward * Time.deltaTime * hittingPointMovingSpeed);
		}
	}
}
