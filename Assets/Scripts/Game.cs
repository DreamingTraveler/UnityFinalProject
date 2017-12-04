using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Game : MonoBehaviour {
    public int homeScore;
    public int visitingScore;
    public int inning = 1;

    private Text pointText;
	// Use this for initialization
	void Start () {
        pointText = GameObject.Find("Point").GetComponent<Text>();
	}
	
	// Update is called once per frame
	void Update () {
        pointText.text = homeScore + " - " + visitingScore;
	}
}
