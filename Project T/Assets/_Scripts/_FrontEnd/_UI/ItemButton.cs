using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemButton : MonoBehaviour {
    // Fils values and adds listeners to buttons.
    public void FillValues(Item item) {
        print("Filling in");
        transform.GetChild(0).GetComponent<Text>().text = item.itemName;
        transform.GetChild(1).GetComponent<Image>().sprite = Resources.Load<Sprite>(item.spritePath);
        transform.GetChild(3).GetComponent<Text>().text = item.description;
        Transform options = transform.GetChild(4);
        switch (item.category) {
            case 0:
                Weapon nItem = item as Weapon;
                options.GetChild(0).GetChild(0).GetComponent<Text>().text = "Equip";
                options.GetChild(0).GetComponent<Button>().onClick.AddListener(() => InventoryManager.thisManager.Use(item));
                break;
            case 1:
                Consumable nnnItem = item as Consumable;
                transform.GetChild(2).GetComponent<Text>().text += "x " + nnnItem.count.ToString();

                options.GetChild(0).GetChild(0).GetComponent<Text>().text = "Consume";
                options.GetChild(0).GetComponent<Button>().onClick.AddListener(() => InventoryManager.thisManager.Use(item));
                break;
            case 2:
                CraftingObject nnItem = item as CraftingObject;
                Destroy(options.GetChild(0).gameObject);
                transform.GetChild(2).GetComponent<Text>().text += "x " + nnItem.count.ToString();
                break;

        }
        options.GetChild(1).GetComponent<Button>().onClick.AddListener(() => InventoryManager.thisManager.Delete(item));
    }
    // Old Functionality 
    /*public void OpenOptions(Item item) {
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
    }*/
    public void UpdateCount(int Count) {
        transform.GetChild(2).GetComponent<Text>().text = "x " + Count.ToString();
    }
}
