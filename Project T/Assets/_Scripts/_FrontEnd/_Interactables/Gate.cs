using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gate : MonoBehaviour {
    int i = 0;
    public void Open() {
        if(i == 2) {
            transform.GetChild(0).transform.GetComponent<Animator>().SetBool("Open", true);
            transform.GetChild(1).transform.GetComponent<Animator>().SetBool("Open", true);
        }
        else {
            i++;
        }
    }
}
