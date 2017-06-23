using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class S2SSound : MonoBehaviour {

    public SoundManager thisManager;

	// Use this for initialization
	void Start () {
        thisManager = GetComponent<SoundManager>();
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetButtonDown("Fire1")) {
            //print("DitDoetHijWel");
            thisManager.audioNumber = 11;
            //print("DitDoetHijOok");
            thisManager.SoundLister();
        }
	}
}
