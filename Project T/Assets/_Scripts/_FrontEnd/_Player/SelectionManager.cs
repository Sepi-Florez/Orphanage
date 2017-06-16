using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectionManager : MonoBehaviour
{
    public LayerMask ignoreLayer;

    public InventoryManager inventoryManager;

    public void Update()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward * 20, out hit, 200))
            Debug.DrawRay(transform.position, transform.forward * 20, Color.green);
        {
            if (Input.GetButtonDown("Fire3"))
            {
                if (hit.transform.tag == "Item")
                {

                    InventoryManager.thisManager.InventoryAdd(hit.transform.GetComponent<ItemPickUp>().itemID, 2);
                    Destroy(hit.transform.gameObject);
                    //inventoryManager.inventory
                    //hit.transform.GetComponent<ItemPickup>().itemID;
                }
                else if (hit.transform.tag == "NPC")
                {

                }
            }

        }
    }
}
