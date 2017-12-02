using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cursor : MonoBehaviour {
	public Vector3 mousePos;
	public Vector3 targetPos;
	public float w, h;

	private GameObject pitcher;
	// Use this for initialization
	void Start () {
		pitcher = GameObject.FindGameObjectWithTag ("Pitcher");
	}
	
	// Update is called once per frame
	void Update () {
		w = Camera.main.pixelWidth;
		h = Camera.main.pixelHeight;

		if (pitcher.GetComponent<Pitch> ().canChooseBall) {
			mousePos = Input.mousePosition;
			targetPos = new Vector3 ();
			targetPos.x = mousePos.x - (w/2);
			targetPos.y = mousePos.y - (h/2);
			transform.localPosition = targetPos;
		}

	}
}
