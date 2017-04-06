using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System;
using System.Xml;
using System.Xml.Serialization;
using UnityEngine.EventSystems;

public class InventoryManager : MonoBehaviour {
    public static InventoryManager thisManager;

    public GameObject buttonPref;
    
    GameObject[] contentObjects = new GameObject[2];

    public 
        GameObject optionWindowPref;


    List<Item> inventory = new List<Item>();
    List<Transform> inventoryButtons = new List<Transform>();
    void Start() {
        InventoryAdd(0,2);
    }

    void Awake() {
        thisManager = this;
        for (int i = 0; i < 2; i++) {
            contentObjects[i] = GameObject.FindGameObjectWithTag("Content" + i);
        }
    }
    // Is called upon when adding an item to the inventory
    void InventoryAdd(int itemID,int count) {
        if (DataBaseManager.thisManager.ReturnItem(itemID) != null) {
            Item newItem = DataBaseManager.thisManager.ReturnItem(itemID);
            switch (newItem.category) {
                case 1:
                    Consumable newCon = newItem as Consumable;
                    newCon.count = count;
                    break;
                case 2:
                    CraftingObject newCraft = newItem as CraftingObject;
                    newCraft.count = count;
                    newItem = newCraft;
                    break;
            }
            inventoryButtons.Add(Visualize(newItem));
            inventory.Add(newItem);
        }

    }
    //Instantiates the button which represents the item.
    Transform Visualize(Item item) {
        Transform newButton = Instantiate(buttonPref, Vector3.zero, Quaternion.identity).transform;
        newButton.SetParent(contentObjects[item.category].transform);
        helpArrange(newButton);
        newButton.GetComponent<ItemButton>().FillValues(item);
        return newButton;
    }
    //Used to delete the item received from the inventory
    public void Delete(Item item) {
        if(item.category == 0) {
            //check if equiped else delete
        }
        else {
            int a = 0;
            foreach(Item i in GetCategory(item.category)) {
                a++;
                print("InvCheck");
                if(inventory[a] == item) {
                    inventory.RemoveAt(a);
                    Destroy(inventoryButtons[a].gameObject);
                    inventoryButtons.RemoveAt(a);
                }
            }
        }
    }
    public void Use(Item item) {
        switch (item.category) {
            case 1:
                break;
            case 2:
                break;
        }
    }
    //Gives back a list full of items which are of the requested category
    List<Item> GetCategory(int category) {
        List<Item> scanList = new List<Item>();
        for (int a = 0; a < inventory.Count; a++) {
            if (inventory[a].category == category) {
                scanList.Add(inventory[a]);
            }
        }
        return scanList;
    }
    public void helpArrange(Transform newButton) {
        newButton.localPosition = Vector3.zero;
        newButton.localRotation = Quaternion.identity;
        newButton.localScale = new Vector3(1, 1, 1);
    }
} 