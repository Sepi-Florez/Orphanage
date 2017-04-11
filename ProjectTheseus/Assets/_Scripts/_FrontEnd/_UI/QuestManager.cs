using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System;
using System.Xml;
using System.Xml.Serialization;

public class QuestManager : MonoBehaviour {
    public static QuestManager thisManager;
    public string dataPath;
    public QuestList questList;

    public Transform questPrefab;

    public int followingQuestID = 0;

    public void Awake() {
        thisManager = this;
        if (File.Exists(Application.persistentDataPath + dataPath)) {
            FileStream stream = new FileStream(Application.persistentDataPath + dataPath, FileMode.Open);
            XmlSerializer reader = new XmlSerializer(typeof(QuestList));
            QuestList a = reader.Deserialize(stream) as QuestList;
            stream.Close();
            questList = a;
        }
        else {
            XmlSerializer writer = new XmlSerializer(typeof(QuestList));
            FileStream stream = new FileStream(Application.persistentDataPath + dataPath, FileMode.Create);
            print(Application.persistentDataPath + dataPath);
            writer.Serialize(stream, questList);
            stream.Close();
        }
    }
    public void Start() {
        QuestAdd(0);
        QuestAdd(1);
        QuestAdd(2);
        QuestAdd(3);
    }
    public void Update() {
        if (Input.GetButtonDown("Fire2")) {
        }
    }
    public void QuestShow(int questID) {
        Transform qi = GameObject.FindGameObjectWithTag("QuestInfo").transform;
        qi.GetChild(0).GetComponent<Text>().text = QuestManager.thisManager.questList.qList[questID].title;
        qi.GetChild(1).GetComponent<Text>().text = QuestManager.thisManager.questList.qList[questID].description;
    }
    public void QuestAdd(int questID) {
        Transform qi = GameObject.FindGameObjectWithTag("QuestContent").transform;
        Transform quest = (Transform)Instantiate(questPrefab, Vector3.zero, Quaternion.identity);
        quest.SetParent(qi);
        InventoryManager.thisManager.helpArrange(quest);
        quest.GetChild(0).GetComponent<Text>().text = questList.qList[questID].title;
        quest.GetComponent<Button>().onClick.AddListener(() => QuestShow(questID));
    }
}
public class QuestList {
    public List<Quest> qList;
    public QuestList() {
        qList = new List<Quest>();
    }
}
public class Quest {
    public string title;
    public string description;
    public Quest() {

    }
    public Quest(string _title, string _description) {
        title = _title;
        description = _description;
    }
}
