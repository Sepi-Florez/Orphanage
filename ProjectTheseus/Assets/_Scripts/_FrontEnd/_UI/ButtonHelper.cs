using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonHelper : MonoBehaviour {
    public int category;
    public int i ;
    void Start() {
        switch (transform.parent.tag) {
            case "materialObject":
                i = InventoryManager.thisManager.materials.Count;
                category = 0;
                break;
            case "equipObject":
                i = InventoryManager.thisManager.equipment.Count;
                category = 1;
                break;

        }
        i--;



    }

    public void OnClickAction() {
        InventoryManager.thisManager.Selected(category,i);
        
    }
    public void OnExitAction() {

    }
}
