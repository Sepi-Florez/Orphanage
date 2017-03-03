using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using System.Xml;
using System.Xml.Serialization;
[Serializable]
public class InventoryManager : MonoBehaviour {

    public List<Item>dbList= new List<Item>();
    public List<Item>inventory = new List<Item>();
	void Awake () {
        FileStream stream = new FileStream(Application.dataPath + "/XML/ItemDatabase.xml", FileMode.Open);
        XmlSerializer reader = new XmlSerializer(typeof(ItemDatabase));
        ItemDatabase a = reader.Deserialize(stream) as ItemDatabase ;
        stream.Close();
        a.itemList = dbList;
    }

	void Update () {

	}
    public void AddItem (int ID, int count) {
        inventory.Add(dbList[ID]);
        inventory[0].count = count;
        Organize();
    }
    public void Organize() {

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

