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

    public Button testButton;
    public int testID;
    public int testCount;

    public GameObject ItemPref;
    public List<Item> materials = new List<Item>();
    public List<Item> equipment = new List<Item>();
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
        equipObj = GameObject.FindGameObjectWithTag("equipObject").transform;
        materialObj = GameObject.FindGameObjectWithTag("materialObject").transform;
    }

	void Update () {
        if (Input.GetButtonDown("Jump")) {
            testButton.onClick.AddListener(() => AddItem(testID, testCount));

        }
	}
    public void AddItem(int ID, int count) {
        if (dbList.itemList.Count > ID) {
            switch (dbList.itemList[ID].category) {
                case 0:
                    bool dub = true;
                    for (int a = 0; materials.Count > a; a++) {

                        if (materials[a].itemName == dbList.itemList[ID].itemName) {
                            materials[a].count += count;
                            Refresh(a);
                            dub = false;
                        }

                    }
                    if (dub) {
                        materials.Add(dbList.itemList[ID]);
                        Visualize(dbList.itemList[ID], count);
                    }
                    break;
                case 1:
                    equipment.Add(dbList.itemList[ID]);
                    equipment[0].count = count;
                    Visualize(dbList.itemList[ID], count);
                    break;
            }


        }
    }
    void Organize() {

    }
    public void Visualize(Item newItem,int count) {
        print("Visualizing");
        Transform insItem = (Transform)Instantiate(ItemPref, Vector3.zero, Quaternion.identity).transform;
        insItem.GetChild(0).GetComponent<Text>().text = newItem.itemName;
        switch (newItem.category) {
            case 0:
                insItem.transform.SetParent(materialObj);
                insItem.transform.GetChild(2).GetComponent<Text>().text = count.ToString();
                insItem.transform.GetChild(3).GetComponent<Text>().text = newItem.description;
                break;
            case 1:
                insItem.transform.SetParent(equipObj);
                insItem.transform.GetChild(2).GetComponent<Text>().text = null;
                insItem.transform.GetChild(3).GetComponent<Text>().text = newItem.description;
                break;
        }
        Organize();

    }
    public void Refresh(int num) {
        materialObj.GetChild(num).transform.GetChild(2).GetComponent<Text>().text = materials[num].count.ToString();
    }
    public void CreateItem(Transform creator) {
        if (creator.GetChild(0).transform.GetChild(2).GetComponent<Text>().text != null) {
            Item newItem = new Item(creator.GetChild(0).transform.GetChild(2).GetComponent<Text>().text, dbList.itemList.Count, Convert.ToInt32(creator.GetChild(1).transform.GetChild(2).GetComponent<Text>().text), creator.GetChild(2).transform.GetChild(2).GetComponent<Text>().text);
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
    [XmlElement("Description")]
    public string description;

    public Item() {

    }
    public Item(string _itemName, int _itemID,int _category, string _description ) {
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

