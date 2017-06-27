using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerChecker : MonoBehaviour {

    BunnyBehaviour bunny;
    GameObject aiManager;
    public Transform[] bHoleArray;
    

	// Use this for initialization
	void Start () {
        aiManager = GameObject.FindGameObjectWithTag("AIManager");
        bunny = aiManager.GetComponent<BunnyBehaviour>();
	}
	
    void OnTriggerEnter(Collider checkerCol) {
        if (checkerCol.tag == "Player") {
            bunny.ChaseMode(bHoleArray);
        }
    }
}
