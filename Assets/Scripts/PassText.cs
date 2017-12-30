using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PassText : MonoBehaviour {
	public InputField teamName;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

	}

	public void StoreTeamName(){
		PlayerPrefs.SetString ("TeamName", teamName.text);
	}
}
