using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Game : MonoBehaviour
{
    public int homeScore;
    public int visitingScore;
    public int inning = 1;
    public bool isTopInning = true;
    public string nowAttack = "visiting";
    public bool isHitting = false;
    public bool isBallFlying = false;
    public bool isBallCameraMoving = false;

    private GameObject HomeRunWall;
    private GameObject pitcher;
    private GameObject ball;
    private Text pointText;
    private Text inningText;
    private Text situation;
    // Use this for initialization
    void Start()
    {
        pitcher = GameObject.FindGameObjectWithTag("Pitcher");
        pointText = GameObject.Find("Point").GetComponent<Text>();
        inningText = GameObject.Find("Inning").GetComponent<Text>();
        situation = GameObject.Find("Situation").GetComponent<Text>();
        HomeRunWall = GameObject.Find("HomerunWall");
    }

    // Update is called once per frame
    void Update()
    {
        SetText();
        if (pitcher.GetComponent<Pitch>().outNum == 3)
        {
            ToNextHalfInning();
        }

        if (isBallFlying)
        {
            JudgeOutBall();
        }

        StrikeoutAndFourBall();
    }

    public void SetBall(GameObject cloneBall)
    {
        isBallFlying = true;
        isBallCameraMoving = true;
        ball = cloneBall;
    }

    private void SetText()
    {
        pointText.text = homeScore + " - " + visitingScore;
        string topBottomSign;
        if (isTopInning)
        {
            topBottomSign = "△";
        }
        else
        {
            topBottomSign = "▽";
        }
        inningText.text = inning + " " + topBottomSign;
    }

    private void ToNextHalfInning()
    {
        pitcher.GetComponent<Pitch>().outNum = 0;
        gameObject.GetComponent<BaseCondition>().SetBase("Empty");
        if (isTopInning)
        {//Top -> Bottom
            nowAttack = "home";
            isTopInning = false;
        }
        else
        {//Bottom -> Top (Change Inning)
            nowAttack = "visiting";
            isTopInning = true;
            inning++;
        }
    }

    public void SetSituation(string occasion)
    {
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

    public void AddPoint(int point)
    {
        if (nowAttack == "home")
        {
            homeScore += point;
        }
        else if (nowAttack == "visiting")
        {
            visitingScore += point;
        }
    }

    public void StrikeoutAndFourBall()
    {
        int strike = pitcher.GetComponent<Pitch>().strike;
        int badBall = pitcher.GetComponent<Pitch>().badBall;
        if (strike == 3)
        {//strikeout!
            SetSituation("Strike Out");
            pitcher.GetComponent<Pitch>().outNum++;
            ToNextPlayer();
        }
        else if (badBall == 4)
        {
            SetSituation("BaseOnBall");
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
                SetSituation("OutBall");
                if (pitcher.GetComponent<Pitch>().strike < 2)
                {
                    pitcher.GetComponent<Pitch>().strike++;
                }
            }
            else if (!isHitting && (ball.transform.position.x > 200f && ball.transform.position.z > 200f))
            {
                pitcher.GetComponent<Pitch>().outNum++;
                ToNextPlayer();
            }
            isHitting = false;
            Invoke("SwitchToPitcherCamera", 3.0f);
        }
    }

    private void SwitchToPitcherCamera()
    {
        isBallCameraMoving = false;
        pitcher.GetComponent<Pitch>().cloneBall.SetActive(false);
        pitcher.GetComponent<Pitch>().EnableChooseButton();
        SetSituation("Clear");
    }
}
