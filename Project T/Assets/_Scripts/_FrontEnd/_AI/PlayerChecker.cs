using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerChecker : MonoBehaviour {

    BunnyBehaviour bunny;

	// Use this for initialization
	void Start () {
        bunny = transform.parent.GetComponent<BunnyBehaviour>();
	}
	
    void OnTriggerEnter() {
        bunny.PlayerHasEntered();
    }
}
