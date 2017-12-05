using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowBall : MonoBehaviour {
    public Vector3 offset;

    private GameObject ballCamera;
    private GameObject ball;
    private GameObject field;
	// Use this for initialization
	void Start () {
        field = GameObject.Find("Field");
        ballCamera = GameObject.Find("Ball_Camera");
    }
	
	// Update is called once per frame
	void Update () {
        if (field.GetComponent<Game>().isBallFlying){
            ballCamera.transform.position = ball.transform.position + offset;
        }
	}

    public void SetBall(GameObject cloneBall) {
        ball = cloneBall;
    }
}
