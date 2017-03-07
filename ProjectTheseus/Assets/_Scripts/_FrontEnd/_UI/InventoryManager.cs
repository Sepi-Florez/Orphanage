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
    public List<Item>inventory = new List<Item>();
	void Awake () {
        if (File.Exists(Application.dataPath + dataPath)) {
            FileStream stream = new FileStream(Application.dataPath + dataPath, FileMode.Open);
            XmlSerializer reader = new XmlSerializer(typeof(ItemDatabase));
            ItemDatabase a = reader.Deserialize(stream) as ItemDatabase;
            stream.Close();
            a = dbList;
            print("Loaded Database");
        }
        else {
            dbList = new ItemDatabase();
            print("Did not load Database");
        }
    }

	void Update () {

	}
    public void AddItem (int ID, int count) {
        inventory.Add(dbList.itemList[ID]);
        inventory[0].count = count;
        Organize();
    }
    public void Organize() {

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
    [XmlElement("Count")]
    public int count;

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

