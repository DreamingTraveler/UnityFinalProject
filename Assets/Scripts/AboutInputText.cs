using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AboutInputText : MonoBehaviour {
	public InputField teamName;
	private string name;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (teamName.text.Length >= 6) {
			teamName.DeactivateInputField ();
		}
	}
}
