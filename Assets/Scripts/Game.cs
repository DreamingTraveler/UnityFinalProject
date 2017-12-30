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
	public int currentInningScore = 0;
	public bool isTopInning = true;//
	public bool isGameEnd = false;
	public string nowAttack = "visitor";//
	public bool isHitting = false;
	public bool isBallFlying = false;
	public bool isBallCameraMoving = false;
	public GameObject changeInningScene;
	public GameObject scoreCanvas;

	public Image ballImg;
	public Image baseOnBallsImg;
	public Image doubleImg;
	public Image foulImg;
	public Image homeRunImg;
	public Image singleImg;
	public Image strikeImg;
	public Image strikeOutImg;
	public Image tripleImg;
	public Image outImg;

	private GameObject HomeRunWall;
	private GameObject pitcher;
	private GameObject ball;

	public Button nextInningBtn;
	public Button playAgainBtn;
	public Button quitBtn;
	public Text visitorName1;
	public Text visitorName2;
    public Text homePointText;
	public Text visitorPointText;
	public Text inningText;
	public Text topInningText;
	public Text bottomInningText;
	public Text outNumText;
	public Text resultText;
	// Use this for initialization
	void Start () {
		pitcher = GameObject.FindGameObjectWithTag ("Pitcher");
		HomeRunWall = GameObject.Find ("HomerunWall");
		visitorName1.text = PlayerPrefs.GetString ("TeamName");
		visitorName2.text = PlayerPrefs.GetString ("TeamName");
		HideImage ();
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
		if (isGameEnd) {
			changeInningScene.SetActive (true);
			resultText.gameObject.SetActive (true);
			nextInningBtn.gameObject.SetActive (false);
			playAgainBtn.gameObject.SetActive (true);
			quitBtn.gameObject.SetActive (true);
			if (visitorScore > homeScore) {
				resultText.text = "YOU WIN!!";
			} else if (visitorScore == homeScore) {
				resultText.text = "DRAW";
			} else {
				resultText.text = "YOU LOSE";
			}
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
		gameObject.GetComponent<ShowScore> ().Show ();
		gameObject.GetComponent<BaseCondition>().SetBase("Empty");
		changeInningScene.SetActive (true);
		scoreCanvas.SetActive (false);
		if (inning == 6) {
			if ((homeScore > visitorScore && isTopInning) || !isTopInning) {
				isGameEnd = true;
			}
		}
	}

	public void HideScene(){
		currentInningScore = 0;
		changeInningScene.SetActive (false);
		scoreCanvas.SetActive (true);
		if (isTopInning) {//Top -> Bottom
			nowAttack = "home";
			isTopInning = false;
			pitcher.GetComponent<Pitch> ().EnableChooseButton ();
		} else {//Bottom -> Top (Change Inning)
			nowAttack = "visitor";
			pitcher.GetComponent<Pitch> ().EnableReadyBtn ();
			isTopInning = true;
			gameObject.GetComponent<SwitchCamera> ().SwitchToHitterCamera ();
			inning++;
		}
	}

	public void ShowImage(string situation){
		if (situation == "Ball") {
			ballImg.gameObject.SetActive (true);
		} else if (situation == "BB") {
			baseOnBallsImg.gameObject.SetActive (true);
		} else if (situation == "Double") {
			doubleImg.gameObject.SetActive (true);
		} else if (situation == "Foul") {
			foulImg.gameObject.SetActive (true);
		} else if (situation == "HomeRun") {
			homeRunImg.gameObject.SetActive (true);
		} else if (situation == "Single") {
			singleImg.gameObject.SetActive (true);
		} else if (situation == "Strike") {
			strikeImg.gameObject.SetActive (true);
		} else if (situation == "StrikeOut") {
			strikeOutImg.gameObject.SetActive (true);
		} else if (situation == "Triple") {
			tripleImg.gameObject.SetActive (true);
		} else if (situation == "Out") {
			outImg.gameObject.SetActive (true);
		}
		Invoke ("HideImage", 1.5f);
	}

	private void HideImage(){
		ballImg.gameObject.SetActive(false);
		baseOnBallsImg.gameObject.SetActive(false);
		doubleImg.gameObject.SetActive(false);
		foulImg.gameObject.SetActive(false);
		homeRunImg.gameObject.SetActive(false);
		singleImg.gameObject.SetActive(false);
		strikeImg.gameObject.SetActive(false);
		strikeOutImg.gameObject.SetActive(false);
		tripleImg.gameObject.SetActive(false);
		outImg.gameObject.SetActive (false);
	}

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
		currentInningScore += point;
    }

    public void StrikeoutAndFourBall(){
        int strike = pitcher.GetComponent<Pitch>().strike;
        int badBall = pitcher.GetComponent<Pitch>().badBall;
        if (strike == 3){//strikeout!
			ShowImage ("StrikeOut");
            outNum++;
            ToNextPlayer();
        }
        else if (badBall == 4){
			ShowImage ("BB");
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
            if ((ball.transform.position.x < 200f || ball.transform.position.z < 200f)){//faul
				ShowImage ("Foul");
                if (pitcher.GetComponent<Pitch>().strike < 2){
                    pitcher.GetComponent<Pitch>().strike++;
                }
            }
			else if ((!isHitting && (ball.transform.position.x > 200f && ball.transform.position.z > 200f)) ||
				GameObject.Find("Hitter").GetComponent<HitBall>().randomY > 600){
                outNum++;
				ShowImage ("Out");
                ToNextPlayer();
            }
            isHitting = false;
            Invoke("SwitchCamera", 1.5f);
        }
    }

    private void SwitchCamera(){
        isBallCameraMoving = false;
        pitcher.GetComponent<Pitch>().cloneBall.SetActive(false);
		if (nowAttack == "visitor") {
			pitcher.GetComponent<Pitch> ().EnableReadyBtn ();
			gameObject.GetComponent<SwitchCamera>().SwitchToHitterCamera();
		} else {
			pitcher.GetComponent<Pitch>().EnableChooseButton();
		}
    }
}
