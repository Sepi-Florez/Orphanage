using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using System.Xml;
using System.Xml.Serialization;

public class DataBaseManager : MonoBehaviour {
    public static DataBaseManager thisManager;

    public ItemDatabase dbList;

    public string dataPath;

    void Awake() {
        thisManager = this;

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
    [XmlElement("Description")]
    public string description;

    public Item() {

    }
    public Item(string _itemName, int _itemID, int _category, string _description) {
        itemName = _itemName;
        itemID = _itemID;
        category = _category;
        description = _description;
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
