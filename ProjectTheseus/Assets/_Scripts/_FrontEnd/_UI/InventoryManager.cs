using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System;
using System.Xml;
using System.Xml.Serialization;
[Serializable]
public class InventoryManager : MonoBehaviour {
    public string dataPath;
    public ItemDatabase dbList;
    Transform inventoryObj;
    public GameObject ItemPref;
    public List<Item>inventory = new List<Item>();
	void Awake () {
        if (File.Exists(Application.dataPath + dataPath)) {
            FileStream stream = new FileStream(Application.dataPath + dataPath, FileMode.Open);
            XmlSerializer reader = new XmlSerializer(typeof(ItemDatabase));
            ItemDatabase a = reader.Deserialize(stream) as ItemDatabase;
            stream.Close();
            dbList = a;
            print("Loaded Database");
        }
        else {
            dbList = new ItemDatabase();
            print("Did not load Database");
        }
        inventoryObj = GameObject.FindGameObjectWithTag("Inventory").transform;
    }

	void Update () {
        if (Input.GetButtonDown("Jump")) {
            AddItem(0, 2);
        }
	}
    public void AddItem (int ID, int count) {
        if (dbList.itemList.Count > ID) {
            inventory.Add(dbList.itemList[ID]);
            inventory[0].count = count;
            Visualize(dbList.itemList[ID],0); 
            Organize();
        }
    }
    void Organize() {

    }
    public void Visualize(Item newItem, int option) {
        Transform insItem = (Transform)Instantiate(ItemPref, inventoryObj.position, Quaternion.identity).transform;
        insItem.transform.SetParent(inventoryObj.GetChild(newItem.category));
        insItem.GetChild(0).GetComponent<Text>().text = newItem.itemName + newItem.count;
    }
    public void CreateItem(Transform creator) {
        if (creator.GetChild(0).transform.GetChild(2).GetComponent<Text>().text != null) {
            Item newItem = new Item();
            newItem.itemName = creator.GetChild(0).transform.GetChild(2).GetComponent<Text>().text;
            newItem.itemID = dbList.itemList.Count;
            dbList.itemList.Add(newItem);
            XmlSerializer writer = new XmlSerializer(typeof(ItemDatabase));
            FileStream stream = new FileStream(Application.dataPath + dataPath, FileMode.Create);
            writer.Serialize(stream, dbList);
            stream.Close();
            print("AddedItem");
        }
        else {
            print("did not add item");
        }

    }
}
[Serializable]
public class Item {
    [XmlElement("ItemName")]
    public string itemName;
    [XmlElement("ItemID")]
    public int itemID;
    public int count;
    [XmlElement("Category")]
    public int category;

    public Item() {

    }
    public Item(string _itemName, int _itemID, int _count) {
        itemName = _itemName;
        itemID = _itemID;
        count = _count;
    }
}
[XmlRoot("ItemDataBase")]
public class ItemDatabase {
    [XmlArray("List")]
    public List<Item> itemList;

    public ItemDatabase() {
        itemList = new List<Item>();
    }

}

