using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class questCollider : MonoBehaviour {
    public bool done;
    public bool give;
    public int questDone;
    public int questGive;
    public bool destroyOnActivation;
    private void OnTriggerEnter(Collider intruder) {

        if(intruder.transform.tag == "Player") {
            if (QuestManager.thisManager.QuestCheck(questDone)) {
                print("checkdone");
                if(done)
                    QuestManager.thisManager.QuestComplete(questDone);
                if(give)
                    QuestManager.thisManager.QuestAdd(questGive);
                if(destroyOnActivation)
                    Destroy(gameObject);
            }
        }
    }
}
