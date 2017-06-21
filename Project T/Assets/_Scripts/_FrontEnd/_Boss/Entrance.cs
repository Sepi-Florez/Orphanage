using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Entrance : MonoBehaviour {
    bool i = false;
    public Transform daddy;

    void Awake() {
        foreach (Transform child in daddy) {
            child.GetChild(0).GetComponent<ParticleSystem>().Stop();
        }
    }

    void OnTriggerEnter(Collider intruder) {
        print("Entered");
        if (intruder.transform.tag == "Player") {
            if (!i) {
                StartCoroutine(StartBoss());
            }
        }
    }
    IEnumerator StartBoss() {
        while (!i) {
            foreach (Transform child in daddy) {
                child.GetChild(0).GetComponent<ParticleSystem>().Play();
                yield return new WaitForSeconds(0.5f);
            }
            print("Start Boss");
            i = true;
        }
    }
}
