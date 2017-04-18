using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class questCollider : MonoBehaviour {
    public int questID;
    private void OnTriggerEnter(Collider intruder) { 
        if(intruder.transform.tag == "Player") {
            if (QuestManager.thisManager.QuestCheck(questID)) {

            }
        }
    }
}
