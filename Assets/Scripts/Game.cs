using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Game : MonoBehaviour
{
    public int homeScore;//
    public int visitorScore;//
    public int inning = 1;//
	public int outNum;
	public int homeHitNum = 0;//
	public int visitorHitNum = 0;//
	public bool isTopInning = true;//
	public string nowAttack = "visitor";//
	public bool isHitting = false;
	public bool isBallFlying = false;
	public bool isBallCameraMoving = false;

	private GameObject HomeRunWall;
	private GameObject pitcher;
	private GameObject ball;
    private Text homePointText;
	private Text visitorPointText;
	private Text inningText;
	private Text topInningText;
	private Text bottomInningText;
	private Text outNumText;
    private Text situation;
	// Use this for initialization
	void Start () {
		pitcher = GameObject.FindGameObjectWithTag ("Pitcher");
		homePointText = GameObject.Find("HomePoint").GetComponent<Text>();
		visitorPointText = GameObject.Find("VisitingPoint").GetComponent<Text>();
		inningText = GameObject.Find ("Inning").GetComponent<Text> ();
        situation = GameObject.Find ("Situation").GetComponent<Text> ();
		topInningText = GameObject.Find ("TopInning").GetComponent<Text> ();
		bottomInningText = GameObject.Find ("BottomInning").GetComponent<Text> ();
		outNumText = GameObject.Find ("Out").GetComponent<Text> ();
		HomeRunWall = GameObject.Find ("HomerunWall");
	}
	
	// Update is called once per frame
	void Update () {
		SetText ();
		if (outNum == 3) {
			ToNextHalfInning ();
		}

		if (isBallFlying) {
			JudgeOutBall ();
		}
		StrikeoutAndFourBall ();
		JudgeWinner ();
		outNumText.text = "Out: " + outNum;
	}

	private void JudgeWinner(){
		if(inning > 6){
			SceneManager.LoadScene (1);
		}
	}

	public void SetBall(GameObject cloneBall){
		isBallFlying = true;
		isBallCameraMoving = true;
		ball = cloneBall;
	}

	private void SetText(){
		homePointText.text = homeScore.ToString();
		visitorPointText.text = visitorScore.ToString();
		if (isTopInning) {
			topInningText.text = "▲";
			bottomInningText.text = "";
		} else {
			topInningText.text = "";
			bottomInningText.text = "▼";
		}
		inningText.text = inning.ToString();
	}

	private void ToNextHalfInning(){
		outNum = 0;
		gameObject.GetComponent<BaseCondition>().SetBase("Empty");
		if (isTopInning) {//Top -> Bottom
			nowAttack = "home";
			pitcher.GetComponent<Pitch> ().EnableChooseButton ();
			isTopInning = false;
		} else {//Bottom -> Top (Change Inning)
			nowAttack = "visitor";
			pitcher.GetComponent<Pitch> ().EnableReadyBtn ();
			gameObject.GetComponent<SwitchCamera> ().SwitchToHitterCamera ();
			isTopInning = true;
			inning++;
		}
		SaveData ();
		SceneManager.LoadScene (2);
	}

	private void SaveData(){
		PlayerPrefs.SetInt ("HomeScore", homeScore);
		PlayerPrefs.SetInt ("VisitorScore", visitorScore);
		PlayerPrefs.SetInt ("Inning", inning);
		PlayerPrefs.SetInt ("HomeHit", homeHitNum);
		PlayerPrefs.SetInt ("VisitorHit", visitorHitNum);

		if (isTopInning) {
			PlayerPrefs.SetInt ("IsTopInning", 1);
		} else {
			PlayerPrefs.SetInt ("IsTopInning", 0);
		}
		PlayerPrefs.SetString ("NowAttack", nowAttack);
	}
	/*
    public void SetSituation(string occasion){
        if (occasion == "Clear")
            situation.text = "";

        else if (occasion == "Strike")
            situation.text = "Strike";
        else if (occasion == "Ball")
            situation.text = "Ball";

        else if (occasion == "Single")
            situation.text = "Single";
        else if (occasion == "Double")
            situation.text = "Double";
        else if (occasion == "Triple")
            situation.text = "Triple";
        else if (occasion == "OutBall")
            situation.text = "Out Ball";
        else if (occasion == "HomeRun")
            situation.text = "HomeRun";

        else if (occasion == "StrileOut")
            situation.text = "Strike Out";
        else if (occasion == "BaseOnBall")
            situation.text = "Base On Ball";
    }
    */

	public void AddHitNum(){
		if (nowAttack == "home") {
			homeHitNum++;
		} else {
			visitorHitNum++;
		}
	}

    public void AddPoint(int point)
    {
        if (nowAttack == "home"){
            homeScore += point;
        }
        else if (nowAttack == "visitor"){
            visitorScore += point;
        }
    }

    public void StrikeoutAndFourBall()
    {
        int strike = pitcher.GetComponent<Pitch>().strike;
        int badBall = pitcher.GetComponent<Pitch>().badBall;
        if (strike == 3)
        {//strikeout!
            //SetSituation("Strike Out");
            outNum++;
            ToNextPlayer();
        }
        else if (badBall == 4)
        {
            //SetSituation("BaseOnBall");
            ToNextPlayer();
            gameObject.GetComponent<BaseCondition>().BaseStateMachine(1);
        }
    }

    public void ToNextPlayer()
    {
        pitcher.GetComponent<Pitch>().strike = 0;
        pitcher.GetComponent<Pitch>().badBall = 0;
    }

    public void JudgeOutBall()
    {
        if (ball.transform.position.y <= 1.0f)
        {//The ball falls to the field
            isBallFlying = false;
            if ((ball.transform.position.x < 200f || ball.transform.position.z < 200f))
            {//faul
                //SetSituation("OutBall");
                if (pitcher.GetComponent<Pitch>().strike < 2)
                {
                    pitcher.GetComponent<Pitch>().strike++;
                }
            }
			else if ((!isHitting && (ball.transform.position.x > 200f && ball.transform.position.z > 200f)) ||
				GameObject.Find("Hitter").GetComponent<HitBall>().randomY > 600)
            {
                outNum++;
                ToNextPlayer();
            }
            isHitting = false;
            Invoke("SwitchCamera", 1.5f);
        }
    }

    private void SwitchCamera()
    {
        isBallCameraMoving = false;
        pitcher.GetComponent<Pitch>().cloneBall.SetActive(false);
		if (nowAttack == "visitor") {
			pitcher.GetComponent<Pitch> ().EnableReadyBtn ();
			gameObject.GetComponent<SwitchCamera>().SwitchToHitterCamera();
		} else {
			pitcher.GetComponent<Pitch>().EnableChooseButton();
		}
        
        //SetSituation("Clear");
    }
}
