using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerChecker : MonoBehaviour {

    BunnyBehaviour bunny;

	// Use this for initialization
	void Start () {
        bunny = transform.parent.GetComponent<BunnyBehaviour>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnTriggerEnter() {
        bunny.PlayerHasEntered();
    }
}
