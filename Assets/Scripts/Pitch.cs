using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Pitch : MonoBehaviour {
	Animator pitcherAnimator;
	Animator ballAnimator;
	public GameObject ball;
    public GameObject strikeZone;
	public GameObject cloneBall;
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
	public int outNum;
	private Vector3 pitchPos;
	private Vector3 tempPos;

    private GameObject field;

	private GameObject hitter;
	private GameObject hittingPoint;
	private GameObject targetPoint;
	private GameObject cursor;
	private MeshRenderer targetMesh;

	private Button confirmBallPos;
	private Button fourSeamBtn;
	private Button sliderBtn;
	private Button cutterBtn;
	private Button forkballBtn;

	private Text strikeBall;
	private Text outNumText;
	private Text speedText;
    // Use this for initialization
    void Start () {
		pitcherAnimator = GameObject.FindGameObjectWithTag("Pitcher").GetComponent<Animator> ();
		pitcherAnimator.Play("Pitcher_Idle");
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
		strikeBall = GameObject.Find ("StrikeBall").GetComponent<Text> ();
		outNumText = GameObject.Find ("Out").GetComponent<Text> ();
		speedText = GameObject.Find ("Speed").GetComponent<Text> ();
    }
		
	// Update is called once per frame
	void Update () {
		if (isPitching) {
			CallHitter (cloneBall);
            Vector3 ballPos = cloneBall.transform.position;
            for (int i = 0; i < 100000; i++){
                if (ballPos.x + ballPos.z >= 404.5f && ballPos.x + ballPos.z <= 415.5f) {
                    RecordBallPos();
                }
            }
            StopBall(cloneBall);
		} 
			
		if (canChooseBall) {
			ChooseBallPosition ();
			targetPoint.transform.position = targetPos;
		} else {
			targetMesh.enabled = false;
			confirmBallPos.gameObject.SetActive (false);
		}
		strikeBall.text = badBall + " - " + strike;
		outNumText.text = "Out: " + outNum;
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
			EnableChooseButton ();
			Destroy (cloneBall);
		}
	}

	public void ChooseBallType(){
		canChooseBall = true;
		hitter.GetComponent<HitBall> ().hitting_force = 0;
	}

	public void CallAnimate(){
		canChooseBall = false;
		DisableChooseButton ();
		hitter.GetComponent<HitBall> ().isSwing = false;
        field.GetComponent<SwitchCamera>().SwitchToHitterCamera();
        strikeZone.SetActive(false);
		Invoke ("PitchAnimate", 2.0f);
	}

	public void EnableChooseButton(){
		fourSeamBtn.gameObject.SetActive (true);
		sliderBtn.gameObject.SetActive (true);
		cutterBtn.gameObject.SetActive (true);
		forkballBtn.gameObject.SetActive (true);
        strikeZone.SetActive(true);
        field.GetComponent<SwitchCamera>().SwitchToPitcherCamera();
        GameObject go = GameObject.FindGameObjectWithTag("Ball");
        Destroy(go);
    }

	private void DisableChooseButton(){
		fourSeamBtn.gameObject.SetActive (false);
		sliderBtn.gameObject.SetActive (false);
		cutterBtn.gameObject.SetActive (false);
		forkballBtn.gameObject.SetActive (false);
	}

	private void PitchAnimate(){
		pitcherAnimator.SetTrigger ("isPitch");

		hittingPoint.transform.position = new Vector3 (-511f, 19.3f, 995f);
		Invoke ("PitchBall", 1.0f);
	}

	public void JudgeBall(){
		Vector3 ballPos = tempPos;
		if (hitter.GetComponent<HitBall> ().isSwing == false) {
			if (ballPos.x >= 198.5f && ballPos.x <= 209.3f && ballPos.y >= 12.5f && ballPos.y <= 25f && 
				ballPos.z >= 199.5f && ballPos.z <= 210.6f) {
				strike++;
			} else {
				badBall++;
			}
		}
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
		hittingPointMovingSpeed = 10000f;
		speed = Random.Range (320f,350f);
	}

	public void SetModeAsSlider(){
		ballMode = 1;
		hittingPointMovingSpeed = 13000f;
		speed = Random.Range (260f,280f);
	}

	public void SetModeAsCutter(){
		ballMode = 2;
		hittingPointMovingSpeed = 11200f;
		speed = Random.Range (300f,320f);
	}

	public void SetModeAsFork(){
		ballMode = 3;
		hittingPointMovingSpeed = 13000f;
		speed = Random.Range (260f,280f);
	}
    //
	private void CallHitter(GameObject cloneBall){
		MoveHittingPoint (cloneBall);
		if (Input.GetKeyDown ("space") && hitter.GetComponent<HitBall>().isSwing == false) {
            if(cloneBall!=null)
            hitter.GetComponent<HitBall>().Swing(cloneBall);
        }
	}
		
	private void MoveHittingPoint(GameObject cloneBall){
        if (!cloneBall) return;
		if (/*cloneBall.transform.position.x + cloneBall.transform.position.z >= 347f &&
			cloneBall.transform.position.x + cloneBall.transform.position.z <= 448f*/
			hitter.GetComponent<HitBall>().CanHit(cloneBall)) {
			//hitting_point.GetComponent<Rigidbody> ().velocity = hitting_point_end.normalized * 100.0f;
			hittingPoint.transform.Translate(Vector3.forward * Time.deltaTime * hittingPointMovingSpeed);
		}
	}
}
