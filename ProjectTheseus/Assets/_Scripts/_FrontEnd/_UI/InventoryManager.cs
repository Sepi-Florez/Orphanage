//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.UI;
//using System.IO;
//using System;
//using System.Xml;
//using System.Xml.Serialization;
//using UnityEngine.EventSystems;
//[Serializable]
//public class InventoryManager : MonoBehaviour {
//    public static InventoryManager thisManager;

//    public string dataPath;
//    public ItemDatabase dbList;
//    Transform equipObj;
//    Transform materialObj;

//    //Temporary Additem variables
//    public int testID;
//    public int testCount;

//    public GameObject ItemPref;
//    public bool select = false;
//    public int selected;
//    public Item selectedItem;
//    public List<Item> materials = new List<Item>();
//    public List<Item> equipment = new List<Item>();
//    void Awake () {
//        thisManager = this;

//        if (File.Exists(Application.dataPath + dataPath)) {
//            FileStream stream = new FileStream(Application.dataPath + dataPath, FileMode.Open);
//            XmlSerializer reader = new XmlSerializer(typeof(ItemDatabase));
//            ItemDatabase a = reader.Deserialize(stream) as ItemDatabase;
//            stream.Close();
//            dbList = a;
//            print("Loaded Database");
//        }
//        else {
//            dbList = new ItemDatabase();
//            print("Did not load Database");
//        }
//        equipObj = GameObject.FindGameObjectWithTag("equipObject").transform;
//        materialObj = GameObject.FindGameObjectWithTag("materialObject").transform;
//    }

//	void Update () {
//        if (Input.GetButtonDown("Jump")) {
//            AddItem(testID, testCount);

//        }
//	}
//    public void Use() {

//    }
//    // Goes through a list of items used to craft an object. It'll get rid of all the items;
//    public void DeleteCrafted(List<Item> CraftList) {
//    }
//    // Deletes item from the inventory
//    public void DeleteItem(int category,int i) {
//        switch (category) {
//            case 0:
//                Destroy(materialObj.GetChild(i).gameObject);
//                materials.RemoveAt(i);
//                for (int a = i + 1; a < materialObj.childCount; a++) {
//                    materialObj.GetChild(a).GetComponent<ButtonHelper>().i--;
//                }
//                break;
//            case 1:
//                Destroy(equipObj.GetChild(i).gameObject);
//                equipment.RemoveAt(i);
//                for (int a = i + 1; a < materialObj.childCount; a++) {
//                    equipObj.GetChild(a).GetComponent<ButtonHelper>().i--;
//                }
//                break;
//        }
//    }
//    //activates when pointerEnters a button 
//    public void Selected(int num) {
//        select = true;
//        switch (SlabManager.thisManager.tabOpen) {
//            case 0:
//                materialObj.GetChild(selected).GetComponent<Image>().color = Color.white;
//                selected = num;
//                materialObj.GetChild(selected).GetComponent<Image>().color = Color.red;
//                selectedItem = materials[selected];
//                break;
//            case 1:
//                equipObj.GetChild(selected).GetComponent<Image>().color = Color.white;
//                selected = num;
//                equipObj.GetChild(selected).GetComponent<Image>().color = Color.red;
//                selectedItem = equipment[selected];
//                break;
//        }
//        print("Selected " + num);
//    }
//    // resets selected for when changing tabs it doesnt stay selected
//    public void ResetSelected() {
//        select = false;
//        switch (SlabManager.thisManager.tabOpen) {
//            case 0:
//                for (int a = 0; a < materialObj.childCount; a++) { 
//                    materialObj.GetChild(a).GetComponent<Image>().color = Color.white;
//                }
//                break;
//            case 1:
//                for (int a = 0; a < materialObj.childCount; a++) {
//                    equipObj.GetChild(a).GetComponent<Image>().color = Color.white;
//                }
//                break;
//        }
//    }
//    public void AddItem(int ID, int count) {
//        if (dbList.itemList.Count > ID && count > 0) {
//            switch (dbList.itemList[ID].category) {
//                case 0:
//                    bool dub = true;
//                    for (int a = 0; materials.Count > a; a++) {
//                        if (materials[a].itemName == dbList.itemList[ID].itemName) {
//                            materials[a].count += count;
//                            Refresh(a);
//                            dub = false;
//                        }
//                    }
//                    if (dub) {
//                        materials.Add(dbList.itemList[ID]);
//                        print(materials.Count);
//                        Visualize(dbList.itemList[ID], count);
//                    }
//                    break;
//                case 1:
//                    equipment.Add(dbList.itemList[ID]);
//                    equipment[0].count = count;
//                    Visualize(dbList.itemList[ID], count);
//                    break;
//            }
//        }
//    }
//    void Organize() {

//    }
//    //Visualizes the items in the UI and gives them the needed information aswell as adding an event trigger to detect selection
//    public void Visualize(Item newItem,int count) {
//        print("Visualizing");
//        Transform insItem = (Transform)Instantiate(ItemPref, Vector3.zero, Quaternion.identity).transform;
//        insItem.GetChild(0).GetComponent<Text>().text = newItem.itemName;
//        switch (newItem.category) {
//            case 0:
//                insItem.transform.SetParent(materialObj);
//                insItem.transform.GetChild(2).GetComponent<Text>().text = count.ToString();
//                insItem.transform.GetChild(3).GetComponent<Text>().text = newItem.description;
//                break;
//            case 1: 
//                insItem.transform.SetParent(equipObj);
//                insItem.transform.GetChild(2).GetComponent<Text>().text = null;
//                insItem.transform.GetChild(3).GetComponent<Text>().text = newItem.description;
//                break;
//        }
//        insItem.localScale = new Vector3(1, 1, 1);
//        insItem.localRotation = Quaternion.Euler(0, 0, 0);
//        insItem.localPosition = new Vector3(0, 0, 0);
//        Organize();

//    }

//    // used to refresh count values of a certain button
//    public void Refresh(int num) {
//        materialObj.GetChild(num).transform.GetChild(2).GetComponent<Text>().text = materials[num].count.ToString();
//    }
//    // used for the ingame creating of items in the database
//    public void CreateItem(Transform creator) {
//        if (creator.GetChild(0).transform.GetChild(2).GetComponent<Text>().text != null) {
//            Item newItem = new Item(creator.GetChild(0).transform.GetChild(2).GetComponent<Text>().text, dbList.itemList.Count, Convert.ToInt32(creator.GetChild(1).transform.GetChild(2).GetComponent<Text>().text), creator.GetChild(2).transform.GetChild(2).GetComponent<Text>().text);
//            dbList.itemList.Add(newItem);
//            XmlSerializer writer = new XmlSerializer(typeof(ItemDatabase));
//            FileStream stream = new FileStream(Application.dataPath + dataPath, FileMode.Create);
//            writer.Serialize(stream, dbList);
//            stream.Close();
//            print("AddedItem");
//        }
//        else {
//            print("did not add item");
//        }

//    }
//}
//[Serializable]
//public class Item {
//    [XmlElement("ItemName")]
//    public string itemName;
//    [XmlElement("ItemID")]
//    public int itemID;
//    public int count;
//    [XmlElement("Category")]
//    public int category;
//    [XmlElement("Description")]
//    public string description;

//    public Item() {

//    }
//    public Item(string _itemName, int _itemID,int _category, string _description ) {
//        itemName = _itemName;
//        itemID = _itemID;
//        category = _category;
//        description = _description;
//    }
//}
//[XmlRoot("ItemDataBase")]
//public class ItemDatabase {
//    [XmlArray("List")]
//    public List<Item> itemList;

//    public ItemDatabase() {
//        itemList = new List<Item>();
//    }

//}

