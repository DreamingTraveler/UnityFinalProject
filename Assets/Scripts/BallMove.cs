using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallMove : MonoBehaviour {
	public int moveSpeed;
	private int currentSpeed;
	Vector3 initPos;
	Rigidbody rigidbody;
	// Use this for initialization
	void Start () {
		rigidbody = GetComponent<Rigidbody> ();
		initPos = gameObject.transform.position;
		currentSpeed = moveSpeed;
	}

	// Update is called once per frame
	void Update () {
		Vector3 pitcherPos = GameObject.FindGameObjectWithTag ("Pitcher").transform.position;
		Vector3 catcherPos = GameObject.FindGameObjectWithTag ("Catcher").transform.position;
		Vector3 ballDirection = -Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.nearClipPlane));
		if (Input.GetButtonDown ("Fire1")) {
			currentSpeed = moveSpeed;
			rigidbody.velocity = ballDirection.normalized * currentSpeed;
		}
	}
	void OnCollisionEnter(Collision col){
		if (col.gameObject.name == "Catcher") {
			gameObject.transform.position = initPos;
			currentSpeed = 0;
		}
	}
}