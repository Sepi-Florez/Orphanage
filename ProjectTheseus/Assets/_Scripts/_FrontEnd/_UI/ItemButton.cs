using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemButton : MonoBehaviour {
    public void FillValues(Item item) {
        transform.GetChild(0).GetComponent<Text>().text = item.itemName;
        transform.GetChild(1).GetComponent<Image>().sprite = Resources.Load<Sprite>(item.spritePath);
        transform.GetChild(3).GetComponent<Text>().text = item.description;
        print(item.spritePath);
        switch (item.category) {
            case 0:
                transform.GetChild(2).GetComponent<Text>().text = "";
                break;
            case 1:
                Consumable nItem = item as Consumable;
                transform.GetChild(2).GetComponent<Text>().text = nItem.count.ToString();
                break;
            case 2:
                break;
        }
        transform.GetComponent<Button>().onClick.AddListener(() => OpenOptions());
    }
    public void OpenOptions() {
        GameObject optionWindow = (GameObject)Instantiate(InventoryManager.thisManager.optionWindowPref, Input.mousePosition, Quaternion.identity);
    }
}
