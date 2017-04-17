using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Quie : MonoBehaviour {
    private void OnTriggerEnter(Collider intruder) { 
        if(intruder.transform.tag == "Player") { 
            if(QuestManager.thisManager.QuestCheck)
        }
    }
}
