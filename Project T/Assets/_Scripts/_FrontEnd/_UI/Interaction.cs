using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interaction : MonoBehaviour {

    public bool it ;
    public float itRange;

    void Update() {
        if (it) {
            RaycastHit hit;
            if(Physics.Raycast(transform.position,transform.forward, out hit, itRange)) {
                Transform itObject = hit.transform;
                switch (itObject.tag) {
                    case "Item":
                        HUDManager.thisManager.Interaction(true, 0);
                        if (Input.GetButtonDown("Interaction")) {
                            InventoryManager.thisManager.InventoryAdd(itObject.GetComponent<itemPickUp>().itemID, itObject.GetComponent<itemPickUp>().count);
                        }
                        break;
                    case "NPC":
                        HUDManager.thisManager.Interaction(true, 1);
                        if (Input.GetButtonDown("Interaction")) {

                        }
                        //activate conversation
                        break;
                    case "Door":
                        HUDManager.thisManager.Interaction(true, 2);
                        if (Input.GetButtonDown("Interaction")) {

                        }
                        //toggle door anim
                        break;
                }
            }
            else {
                HUDManager.thisManager.Interaction(false, 0);
            }
        }
    }
}
