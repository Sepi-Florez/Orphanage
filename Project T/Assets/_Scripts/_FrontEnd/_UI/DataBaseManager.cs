using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using System.Xml;
using System.Xml.Serialization;
public class DataBaseManager : MonoBehaviour {
    public int itemType;
    public string itemName;
    public string itemSpritePath;
    public string itemDescription;
    public int weaponDamage;
    public int count;
    public int effect;
    public int strength;

    bool loaded = false;
    
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
            //print("Loaded Database");
            loaded = true;
        }
        else {
            dbList = new ItemDatabase();
            //print("Did not load Database");
            loaded = false;
        }
    }
    void Start() {
    }
    void Update() {
    }
    // Gives an Item variable back 
    public Item ReturnItem(int itemID) {
        if(loaded)
            if(dbList.itemList.Count > itemID)
                return dbList.itemList[itemID];
        return null;
    }
    void DataBaseAdd() {
        Item newItem = null;
        switch (itemType) {
            case 0:
                newItem = new Weapon(weaponDamage, itemName, itemSpritePath, itemDescription);
                break;
            case 1:
                newItem = new Consumable(strength,effect,count, itemName, itemSpritePath, itemDescription);
                break;
            case 2:
                newItem = new CraftingObject(count, itemName, itemSpritePath, itemDescription);
                break;
            case 3:
                break;
                
        }
        newItem.ID = dbList.itemList.Count;
        dbList.itemList.Add(newItem);
        XmlSerializer writer = new XmlSerializer(typeof(ItemDatabase));
        FileStream stream = new FileStream(Application.dataPath + dataPath, FileMode.Create);
        print(Application.persistentDataPath + dataPath);
        writer.Serialize(stream, dbList);
        stream.Close();
        print("added");
        }
    public Item GetItem(int itemID) {
        return dbList.itemList[itemID];
    }
}
[XmlRoot("ItemDataBase")]
public class ItemDatabase {
    [XmlArray("DataBase")]
    public List<Item> itemList;

    public ItemDatabase() {
        itemList = new List<Item>();
    }

}
[Serializable]
[XmlInclude(typeof(Weapon))]
[XmlInclude(typeof(Consumable))]
[XmlInclude(typeof(CraftingObject))]
public class Item {
    [XmlElement("ID")]
    public int ID;
    [XmlElement("ItemName")]
    public string itemName;
    [XmlElement("spritePath")]
    public string spritePath;
    [XmlElement("Description")]
    public string description;
    [XmlElement("Category")]
    public int category;
    [XmlElement("Components")]
    public int[] comps = new int[5];
    [XmlElement("Components_Need")]
    public int[] compsc = new int[5];

    public Item() {

    }
}
[Serializable]
public class Weapon : Item {
    [XmlElement("Damage")]
    public int weaponDamage;
    public Weapon() {

    }
    public Weapon(int _weaponDamage, string _itemName, string _spritePath, string _description) {
        weaponDamage = _weaponDamage;
        itemName = _itemName;
        spritePath = _spritePath;
        description = _description;
        category = 0;
    }
}
[Serializable]
public class Consumable : Item {
    [XmlElement("Consumable_Strength")]
    public int strength;
    [XmlElement("Consumable_Effect")]
    public int effect;
    [XmlElement("Count")]
    public int count;

    public Consumable() {

    }
    public Consumable(int _strength, int _effect, int _count, string _itemName, string _spritePath, string _description) {
        strength = _strength;
        effect = _effect;
        count = _count;
        itemName = _itemName;
        spritePath = _spritePath;
        description = _description;
        category = 1;
    }
}
[Serializable]
public class CraftingObject : Item {
    [XmlElement("Count")]
    public int count;   
    public CraftingObject() {

    }
    public CraftingObject(int _count, string _itemName, string _spritePath, string _description) {
        count = _count;
        itemName = _itemName;
        spritePath = _spritePath;
        description = _description;
        category = 2;
    }
}
