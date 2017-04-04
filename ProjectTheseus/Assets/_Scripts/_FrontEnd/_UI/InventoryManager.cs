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

    GameObject[] contentObjects;


    List<Item> inventory = new List<Item>();
    List<Transform> inventoryButtons = new List<Transform>();
    void Start() {
        InventoryAdd(0);
    }
 
    void Awake() {
        thisManager = this;
        contentObjects = GameObject.FindGameObjectsWithTag("Content");
    }
    // Is called upon when adding an item to the inventory
    void InventoryAdd(int itemID) {
        Item newItem = DataBaseManager.thisManager.ReturnItem(itemID);
        inventory.Add(newItem);
        inventoryButtons.Add(Visualize(newItem));
    }
    //Instantiates the button which represents the item.
    Transform Visualize(Item item) {
        Transform newButton = Instantiate(buttonPref, Vector3.zero, Quaternion.identity).transform;
        newButton.SetParent(contentObjects[item.category].transform);
        newButton.localPosition = Vector3.zero;
        newButton.localRotation = Quaternion.identity;
        newButton.localScale = new Vector3(1, 1, 1);
        newButton.GetComponent<ItemButton>().FillValues(item.itemName, item.description);

        return newButton;

    }
    void Delete(Item item) {

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
} 