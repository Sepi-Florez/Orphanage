using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class questCollider : MonoBehaviour {
    public int questDone;
    public int questGive;
    private void OnTriggerEnter(Collider intruder) {

        if(intruder.transform.tag == "Player") {
            print("INTRUDr");
            if (QuestManager.thisManager.QuestCheck(questDone)) {
                print("checkdone");
                QuestManager.thisManager.QuestComplete(questDone);
                QuestManager.thisManager.QuestAdd(questGive);
                Destroy(gameObject);
            }
        }
    }
}
