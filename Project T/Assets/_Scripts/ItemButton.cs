using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemButton : MonoBehaviour {

    public void FillValues(Item item) {
        transform.GetChild(0).GetComponent<Text>().text = item.itemName;
        transform.GetChild(1).GetComponent<Image>().sprite = Resources.Load<Sprite>(item.spritePath);
        transform.GetChild(3).GetComponent<Text>().text = item.description;
        switch (item.category) {
            case 1:
                Consumable nItem = item as Consumable;
                transform.GetChild(2).GetComponent<Text>().text += "x " + nItem.count.ToString();
                break;
            case 2:
                CraftingObject nnItem = item as CraftingObject;
                transform.GetChild(2).GetComponent<Text>().text += "x " + nnItem.count.ToString();
                break;
        }
        transform.GetComponent<Button>().onClick.AddListener(() => OpenOptions(item));
    }
    public void OpenOptions(Item item) {
        GameObject optionWindow = (GameObject)Instantiate(InventoryManager.thisManager.optionWindowPref, Input.mousePosition, Quaternion.identity);
        optionWindow.transform.SetParent(GameObject.FindGameObjectWithTag("HUD").transform);
        optionWindow.transform.position = Input.mousePosition;
        switch (item.category) {
            case 0:
                optionWindow.transform.GetChild(1).GetComponent<Button>().onClick.AddListener(() => InventoryManager.thisManager.Use(item));
                optionWindow.transform.GetChild(1).GetChild(0).GetComponent<Text>().text = "Equip";
                break;
            case 1:
                optionWindow.transform.GetChild(1).GetComponent<Button>().onClick.AddListener(() => InventoryManager.thisManager.Use(item));
                optionWindow.transform.GetChild(1).GetChild(0).GetComponent<Text>().text = "Consume";
                break;
            case 2:
                Destroy(optionWindow.transform.GetChild(1).gameObject);
                break;
        }
        optionWindow.transform.GetChild(2).GetChild(0).GetComponent<Text>().text = "Destroy";
        optionWindow.transform.GetChild(2).GetComponent<Button>().onClick.AddListener(() => InventoryManager.thisManager.Delete(item));
        optionWindow.transform.GetChild(3).GetChild(0).GetComponent<Text>().text = "Cancel";
        for(int a = 0; a < optionWindow.transform.childCount; a++) {
            optionWindow.transform.GetChild(a).GetComponent<Button>().onClick.AddListener(() => CloseOptions(optionWindow));
        }
        print("OptionsAdded");
    }
    public void CloseOptions(GameObject options) {
        Destroy(options);
    }
    public void UpdateCount(int Count) {
        transform.GetChild(2).GetComponent<Text>().text = "x " + Count.ToString();
    }
}
