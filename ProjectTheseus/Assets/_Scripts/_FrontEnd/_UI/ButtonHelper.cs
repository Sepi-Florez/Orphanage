using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonHelper : MonoBehaviour {
    public int i ;
    void Start() {
        switch (transform.parent.tag) {
            case "materialObject":
                i = InventoryManager.thisManager.materials.Count;
                break;
            case "equipObject":
                i = InventoryManager.thisManager.equipment.Count;
                break;

        }
        i--;



    }

    public void OnClickAction() {
        InventoryManager.thisManager.Selected(i);
        
    }
}
