using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class questCollider : MonoBehaviour {
    public bool done;
    public bool give;
    public int questDone;
    public int questGive;
    private void OnTriggerEnter(Collider intruder) {

        if(intruder.transform.tag == "Player") {
            print("INTRUDr");
            if (QuestManager.thisManager.QuestCheck(questDone)) {
                print("checkdone");
                if(done)
                QuestManager.thisManager.QuestComplete(questDone);
                if(give)
                QuestManager.thisManager.QuestAdd(questGive);
                Destroy(gameObject);
            }
        }
    }
}
