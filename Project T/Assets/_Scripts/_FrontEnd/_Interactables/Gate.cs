using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gate : MonoBehaviour {
    public int i = 0;
    public void Open() {
        if(i == 1) {
            transform.GetComponent<Animator>().SetBool("Open", true);
            QuestManager.thisManager.QuestComplete(2);
            QuestManager.thisManager.QuestAdd(4);
        }
        else {
            i++;
        }

    }
}
