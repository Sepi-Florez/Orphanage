using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RecipeButton : MonoBehaviour {
    Item thisItem;

    public void FillValues(Item item) {
        thisItem = item;
        transform.GetChild(0).GetChild(0).GetComponent<Image>().sprite = Resources.Load<Sprite>(item.spritePath);
        transform.GetChild(0).GetChild(1).GetComponent<Text>().text = item.itemName;
        UpdateValues();
        Transform mats = transform.GetChild(1);
        if(item.comps.Length < mats.childCount) {
            for (int a = 5 - item.comps.Length; a > 0; a--) {
                print("destroying item");
                Destroy(mats.GetChild(5 - a).gameObject);
            }
        }
        transform.GetComponent<Button>().onClick.AddListener(() => CraftingManager.thisManager.MakeRecipe(thisItem.ID));
    }

    public void UpdateValues() {
        print("UpdatingValues");
        Transform mats = transform.GetChild(1);
        for (int i = 0; i < thisItem.comps.Length; i++) {
            Item newItem = DataBaseManager.thisManager.GetItem(thisItem.comps[i]);
            print(newItem.ID + " = newItem");
            mats.GetChild(i).GetChild(0).GetComponent<Image>().sprite = Resources.Load<Sprite>(newItem.spritePath);
            mats.GetChild(i).GetChild(1).GetComponent<Text>().text = newItem.itemName;
            if (InventoryManager.thisManager.Search(newItem.ID) != null) {
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
            }
            else {
                mats.GetChild(i).GetChild(2).GetComponent<Text>().text = "0";
            }
            mats.GetChild(i).GetChild(3).GetComponent<Text>().text = thisItem.compsc[i].ToString();
        }
    }
}
