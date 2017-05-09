using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RecipeButton : MonoBehaviour {
    int thisID;

    public void FillValues(Item item) {
        thisID = item.ID;
        transform.GetChild(0).GetChild(0).GetComponent<Image>().sprite = Resources.Load<Sprite>(item.spritePath);
        transform.GetChild(0).GetChild(1).GetComponent<Text>().text = item.itemName;
        Transform mats = transform.GetChild(1);
        for(int i = 0; i == item.comps.Length; i++) {
            Item newItem = DataBaseManager.thisManager.GetItem(item.comps[i]);
            mats.GetChild(i).GetChild(0).GetComponent<Image>().sprite = Resources.Load<Sprite>(newItem.spritePath);
            mats.GetChild(i).GetChild(1).GetComponent<Text>().text = newItem.itemName;
            switch (newItem.category) {
                case 1:
                    Consumable newCon = InventoryManager.thisManager.Search(newItem.ID) as Consumable;
                    mats.GetChild(i).GetChild(2).GetComponent<Text>().text = newCon.count.ToString();
                    break;
                case 2:
                    CraftingObject newCra = InventoryManager.thisManager.Search(newItem.ID) as CraftingObject;
                    mats.GetChild(i).GetChild(2).GetComponent<Text>().text = newCra.count.ToString();
                    break;

            }
            mats.GetChild(i).GetChild(3).GetComponent<Text>().text = item.compsc[i].ToString();
        }
        //if(item.comps.Length < mats.childCount) {
        //    for (int a = 5 - item.comps.Length; a == 0; a--) {
        //        Destroy(mats.GetChild(mats.childCount - 1));
        //    }
        //}
    }
    public void UpdateValues() {

    }
}
