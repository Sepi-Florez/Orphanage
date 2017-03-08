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
    Transform equipObj;
    Transform materialObj;

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
        equipObj = GameObject.FindGameObjectWithTag("equipObject").transform;
        materialObj = GameObject.FindGameObjectWithTag("materialObject").transform;
    }

	void Update () {
        if (Input.GetButtonDown("Jump")) {
            AddItem(0, 2);
        }
        if (Input.GetButtonDown("Fire1")) {
            AddItem(1, 10);
        }
        if (Input.GetButtonDown("Fire2")) {
            AddItem(2, 10);
        }
	}
    public void AddItem (int ID, int count) {
        if (dbList.itemList.Count > ID) {
            switch (dbList.itemList[ID].category) {
                case 0:
                    bool dub = true;
                    for(int a = 0; inventory.Count > a; a++) {

                        if(inventory[a].itemName == dbList.itemList[ID].itemName) {
                            inventory[a].count += count;
                            Refresh(a);
                            dub = false;
                        }
                        
                    }
                    if (dub) {
                        CreateAdd(ID, count);
                    }
                    break;
                case 1:
                    CreateAdd(ID, count);
                    break;
            }

        }
    }
    public void CreateAdd(int ID, int count) {
        inventory.Add(dbList.itemList[ID]);
        inventory[0].count = count;
        Visualize(dbList.itemList[ID]);
    }

    void Organize() {

    }
    public void Visualize(Item newItem) {
        print("Visualizing");
        Transform insItem = (Transform)Instantiate(ItemPref, inventoryObj.position, Quaternion.identity).transform;
        insItem.GetChild(0).GetComponent<Text>().text = newItem.itemName;
        switch (newItem.category) {
            case 0:
                insItem.transform.SetParent(equipObj);
                insItem.transform.GetChild(2).GetComponent<Text>().text = inventory[0].count.ToString();
                break;
            case 1:
                insItem.transform.SetParent(materialObj);
                break;
        }
        Organize();

    }
    public void Refresh(int num) {
        equipObj.GetChild(num).transform.GetChild(2).GetComponent<Text>().text = inventory[num].count.ToString();
    }
    public void CreateItem(Transform creator) {
        if (creator.GetChild(0).transform.GetChild(2).GetComponent<Text>().text != null) {
            Item newItem = new Item();
            newItem.itemName = creator.GetChild(0).transform.GetChild(2).GetComponent<Text>().text;
            newItem.category = Convert.ToInt32(creator.GetChild(1).transform.GetChild(2).GetComponent<Text>().text);
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

