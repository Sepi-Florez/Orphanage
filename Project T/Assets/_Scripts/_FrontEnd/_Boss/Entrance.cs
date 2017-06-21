using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Entrance : MonoBehaviour {
    bool i = false;
    public Transform daddyl;
    public Transform daddyr;
    public float torchWait;

    void Awake() {
        for (int i = 0; i <= daddyl.childCount - 1; i++) {
            daddyr.GetChild(i).GetChild(0).GetComponent<ParticleSystem>().Stop();
            daddyl.GetChild(i).GetChild(0).GetComponent<ParticleSystem>().Stop();
        }
    }

    void OnTriggerEnter(Collider intruder) {
        print("Entered");
        if (intruder.transform.tag == "Player") {
            GetComponent<Collider>().enabled = false;
            if (!i) {
                StartCoroutine(StartBoss());
            }
        }
    }
    IEnumerator StartBoss() {
        while (!i) {
            for(int i = 0; i <= daddyl.childCount - 1; i++) {
                daddyr.GetChild(i).GetChild(0).GetComponent<ParticleSystem>().Play();
                daddyl.GetChild(i).GetChild(0).GetComponent<ParticleSystem>().Play();
                yield return new WaitForSeconds(torchWait);
            }
            FindObjectOfType<Boss>().StartLooking();
            HUDManager.thisManager.UpdateBossHealth(100);
            i = true;
        }
    }
}
