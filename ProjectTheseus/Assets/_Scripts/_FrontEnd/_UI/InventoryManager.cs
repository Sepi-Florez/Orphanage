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

    GameObject buttonPref;

    GameObject[] contentObjects;


    List<Item> inventory = new List<Item>();
    List<Transform> inventoryButtons = new List<Transform>();
    void Awake() {
        thisManager = this;
        contentObjects = GameObject.FindGameObjectsWithTag("Content");
    }
    void InventoryAdd(int itemID) {
        Item newItem = DataBaseManager.thisManager.ReturnItem(itemID);
        inventory.Add(newItem);
        inventoryButtons.Add(Visualize(newItem));
    }
    Transform Visualize(Item item) {
        Transform newButton = Instantiate(buttonPref, Vector3.zero, Quaternion.identity).transform;
        newButton.SetParent(contentObjects[item.category].transform);
        return newButton;

    }
    void Delete(Item item) {

    }
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