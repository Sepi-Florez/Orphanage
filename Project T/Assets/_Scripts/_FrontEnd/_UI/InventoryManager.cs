﻿using System.Collections;
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
    
    GameObject[] contentObjects = new GameObject[3];

    public GameObject optionWindowPref;


    List<Item> inventory = new List<Item>();
    public List<Transform> inventoryButtons = new List<Transform>();
    void Update() {

    }

    void Awake() {
        thisManager = this;
        for (int i = 0; i < 3; i++) {
            contentObjects[i] = GameObject.FindGameObjectWithTag("Content" + i);
        }
    }
    private void Start() {
        contentObjects[0].transform.position = new Vector3(0, 0, 0);

    }
    // Is called upon when adding an item to the inventory
    public void InventoryAdd(int item,int count) {

        if (DataBaseManager.thisManager.ReturnItem(item) != null) {
            print("Adding" + item);
            Item newItem = DataBaseManager.thisManager.ReturnItem(item);
            int i = SearchInventory(newItem);
            switch (newItem.category) {
                case 1:
                    Consumable newCon = newItem as Consumable;
                    if (i != -1) {
                        newCon = inventory[i] as Consumable;
                        newCon.count += count;
                        inventoryButtons[i].GetComponent<ItemButton>().UpdateCount(newCon.count);
                        CraftingManager.thisManager.RefreshRecipes();
                        return;
                    }
                    else {
                        newCon.count = count;
                        newItem = newCon;
                    }
                    break;
                case 2:
                    CraftingObject newCraft = newItem as CraftingObject;
                    if (i != -1) {
                        newCraft = inventory[i] as CraftingObject;
                        newCraft.count += count;
                        inventoryButtons[i].GetComponent<ItemButton>().UpdateCount(newCraft.count);
                        CraftingManager.thisManager.RefreshRecipes();
                        return;
                    }
                    else {
                        newCraft.count = count;
                        newItem = newCraft;
                    }
                    break;
            }
            inventoryButtons.Add(Visualize(newItem));
            inventory.Add(newItem);
            CraftingManager.thisManager.RefreshRecipes();

        }

    }
    //Instantiates the button which represents the item.
    Transform Visualize(Item item) {
        print("Vizualizing " + item.itemName);
        Transform newButton = Instantiate(buttonPref, Vector3.zero, Quaternion.identity).transform;
        newButton.SetParent(contentObjects[item.category].transform);
        helpArrange(newButton);
        newButton.GetComponent<ItemButton>().FillValues(item);
        return newButton;
    }
    //Used to delete the item received from the inventory
    public void Delete(Item item) {
        print("Check Delete");
        if(item.category == 0) {
            //check if equiped else delete
        }
        else {
            int a = 0;
            int aa = Search2(item.ID);
            List<Item> checkList = GetCategory(item.category);
            foreach(Item i in checkList) {

                //print("InvCheck");
                if(i == item) {
                    print(i.itemName);
                    inventory.RemoveAt(aa);
                    Destroy(inventoryButtons[aa].gameObject);
                    inventoryButtons.RemoveAt(aa);
                }
                a++;

            }
        }
        CraftingManager.thisManager.RefreshRecipes();

    }
    // Used to remove an fixed amount of an item.
    public void Remove(Item item, int amount) {

        Item newItem = Search(item.ID);
        CraftingObject newCO = newItem as CraftingObject;
        newCO.count -= amount;


        if(newCO.count <= 0) {
            Delete(newCO);
        }
        else {
            inventoryButtons[SearchInventory(item)].GetComponent<ItemButton>().UpdateCount(newCO.count);
        }

    }
    //Activated when consuming or equiping an item.
    public void Use(Item item) {
        switch (item.category) {
            case 0:
                //Equip Weapon
                break;
            case 1:
                //Put Potions Functions here!
                Consumable itemCon = inventory[SearchInventory(item)] as Consumable;
                if(itemCon.count - 1 == 0) {
                    Delete(itemCon);
                }
                else {
                    itemCon.count--;
                    inventoryButtons[SearchInventory(item)].GetComponent<ItemButton>().UpdateCount(itemCon.count);

                }
                break;
        }
        CraftingManager.thisManager.RefreshRecipes();
    }
    //Searches through the inventory list for an specific item and returns it
    public Item Search(int itemID) {
        Item newItem = DataBaseManager.thisManager.GetItem(itemID);
        int a = 0;
        foreach(Item item in inventory) {
            if(newItem.ID == item.ID) {
                return (item);
            }
            a++;
        }
        return null;
    }
   
    public int Search2(int itemID) {
        Item newItem = DataBaseManager.thisManager.GetItem(itemID);
        int a = 0;
        foreach (Item item in inventory) {
            if (newItem.ID == item.ID) {
                return (a);
            }
            a++;
        }
        return -1;
    }
    //checks if a recipe can be made
    public bool RecipeCheck(List<Item> recipe ,List<int> recipeAmount){
        bool ii = false;
		int i = 0;
        int a = 0;
		foreach(Item it in recipe){
			foreach(Item itt in inventory){
                CraftingObject newCon = itt as CraftingObject;
				if(it.ID == itt.ID && recipeAmount[a] <= newCon.count ){
					i++;
				}
                else {
                    print("Recipe is no go");
                }
			}
            a++;
		}
		if(i == recipe.Count){
            ii = true;
		}
        return ii;
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
    //Gives back the index of the given item.
    int SearchInventory(Item item) {
        int i = 0;
        foreach(Item it in inventory) {
            if (it == item) {
                return i;
            }
            i++;
        }
        return -1;
    }
} 