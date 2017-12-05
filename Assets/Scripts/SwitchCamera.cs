using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchCamera : MonoBehaviour {
    public Camera pitcherCamera;
    public Camera hitterCamera;
    public Camera ballCamera;

    private AudioListener pitcherAL;
    private AudioListener hitterAL;
    private AudioListener ballAL;
    // Use this for initialization
    void Start () {
        pitcherAL = pitcherCamera.GetComponent<AudioListener>();
        hitterAL = hitterCamera.GetComponent<AudioListener>();
        ballAL = ballCamera.GetComponent<AudioListener>();

        SwitchToPitcherCamera();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void SwitchToPitcherCamera(){
        pitcherCamera.gameObject.SetActive(true);
        pitcherAL.enabled = true;
        hitterCamera.gameObject.SetActive(false);
        hitterAL.enabled = false;
        ballCamera.gameObject.SetActive(false);
        ballAL.enabled = false;
    }

    public void SwitchToHitterCamera(){
        pitcherCamera.gameObject.SetActive(false);
        pitcherAL.enabled = false;
        hitterCamera.gameObject.SetActive(true);
        hitterAL.enabled = true;
        ballCamera.gameObject.SetActive(false);
        ballAL.enabled = false;
    }

    public void SwitchToBallCamera() {
        pitcherCamera.gameObject.SetActive(false);
        pitcherAL.enabled = false;
        hitterCamera.gameObject.SetActive(false);
        hitterAL.enabled = false;
        ballCamera.gameObject.SetActive(true);
        ballAL.enabled = true;
    }
}
