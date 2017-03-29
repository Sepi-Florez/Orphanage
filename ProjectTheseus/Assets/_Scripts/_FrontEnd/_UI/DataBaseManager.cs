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
    }
    void DataBaseAdd() {
        Item newItem = null;
        switch (itemType) {
            case 0:
                newItem = new Weapon(weaponDamage);
                break;
            case 1:
                newItem = new Consumable(strength,effect,count);
                break;
            case 2:
                newItem = new CraftingObject(count);
                break;
                
        }
        dbList.itemList.Add(newItem);
        XmlSerializer writer = new XmlSerializer(typeof(ItemDatabase));
        FileStream stream = new FileStream(Application.dataPath + dataPath, FileMode.Create);
        writer.Serialize(stream, dbList);
        stream.Close();
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
[Serializable]
public class Item {
    [XmlElement("ItemName")]
    public string itemName;
    [XmlElement("spritePath")]
    public string spritePath;
    [XmlElement("Description")]
    public string description;

    public Item() {

    }
}
public class Weapon : Item {
    int weaponDamage;
    public Weapon(int _weaponDamage) {
        weaponDamage = _weaponDamage;

    }
    public void Item(string _itemName,string _spritePath, string _description) {
        itemName = _itemName;
        description = _description;
    }
}
public class Consumable : Item {
    int strength;
    int effect;
    int count;
    public Consumable(int _strength, int _effect, int _count) {
        strength = _strength;
        effect = _effect;
        count = _count;
    }
}

public class CraftingObject : Item {
    int count;
    public CraftingObject(int _count) {
        count = _count;
    }
}
