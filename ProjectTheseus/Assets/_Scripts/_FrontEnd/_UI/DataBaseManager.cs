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
    void Update() {
        if (Input.GetButtonDown("Jump")) {
            DataBaseAdd();
        }
        if (Input.GetButtonDown("Fire1")) {
            FileStream stream = new FileStream(Application.dataPath + dataPath, FileMode.Open);
            XmlSerializer reader = new XmlSerializer(typeof(ItemDatabase));
            print(dbList.itemList[0].count);
            ItemDatabase a = reader.Deserialize(stream) as ItemDatabase;
            stream.Close();
        }
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
        FileStream stream = new FileStream(Application.persistentDataPath + dataPath, FileMode.Create);
        print(Application.persistentDataPath + dataPath);
        writer.Serialize(stream, dbList);
        stream.Close();
        print("added");
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
    }
}
[Serializable]
public class CraftingObject : Item {
    [XmlElement("Count")]
    int count;
    public CraftingObject() {

    }
    public CraftingObject(int _count, string _itemName, string _spritePath, string _description) {
        count = _count;
        itemName = _itemName;
        spritePath = _spritePath;
        description = _description;
    }
}
