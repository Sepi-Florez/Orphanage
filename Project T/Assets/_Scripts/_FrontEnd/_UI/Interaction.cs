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
                print("Interactle BOy");
                Transform itObject = hit.transform;
                bool i = false;
                switch (itObject.tag) {
                    case "Item":
                        i = true;
                        HUDManager.thisManager.Interaction(true, 0);
                        if (Input.GetButtonDown("Interaction")) {
                            print(itObject.name);
                            InventoryManager.thisManager.InventoryAdd(itObject.GetComponent<itemPickUp>().itemID, itObject.GetComponent<itemPickUp>().count);
                        }
                        break;
                    case "NPC":
                        i = true;
                        HUDManager.thisManager.Interaction(true, 1);
                        if (Input.GetButtonDown("Interaction")) {

                        }
                        //activate conversation
                        break;
                    case "Door":
                        i = true;
                        HUDManager.thisManager.Interaction(true, 0);
                        if (Input.GetButtonDown("Interaction")) {
                            hit.transform.parent.GetComponent<Animator>().SetTrigger("Toggle");
                        }
                        //toggle door anim
                        break;
                    case "Brazier":
                        i = true;
                        HUDManager.thisManager.Interaction(true, 0);
                        if (Input.GetButtonDown("Interaction")) {
                            if(InventoryManager.thisManager.Search(itObject.GetComponent<itemPickUp>().itemID) != null) {
                                InventoryManager.thisManager.Remove(DataBaseManager.thisManager.GetItem(itObject.GetComponent<itemPickUp>().itemID), 1);
                                itObject.GetChild(0).GetComponent<ParticleSystem>().Play();
                                FindObjectOfType<Gate>().Open();
                                
                            }
                        }
                        //toggle door anim
                        break;

                }
                if (!i) {
                    HUDManager.thisManager.Interaction(false, 0);
                }
            }
            else {
                HUDManager.thisManager.Interaction(false, 0);
            }
        }
    }
}
