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

    Transform questContent;
    Transform questInfo;

    public Transform questPrefab;
    public List<Quest> playerQuests = new List<Quest>();
    public List<Quest> playerQuestsCompleted = new List<Quest>();

    public void Awake() {
        thisManager = this; 
        questContent = GameObject.FindGameObjectWithTag("QuestContent").transform;
        questInfo = GameObject.FindGameObjectWithTag("QuestInfo").transform;
        if (File.Exists(Application.persistentDataPath + dataPath)) {
            FileStream stream = new FileStream(Application.persistentDataPath + dataPath, FileMode.Open);
            XmlSerializer reader = new XmlSerializer(typeof(QuestList));
            QuestList a = reader.Deserialize(stream) as QuestList;
            stream.Close();
            questList = a;
            print("Found Quest file");
        }
        else {
            XmlSerializer writer = new XmlSerializer(typeof(QuestList));
            FileStream stream = new FileStream(Application.persistentDataPath + dataPath, FileMode.Create);
            print(Application.persistentDataPath + dataPath);
            writer.Serialize(stream, questList);
            stream.Close();
            print("didn't find Quest file");
        }
    }
    public void Start() {
        QuestAdd(0);
        QuestAdd(1);
        QuestAdd(2);
        QuestComplete(2);
        QuestShow(0);
    }
    public void Update() {
    }
    public void QuestShow(int questID) {
        questInfo.GetChild(0).GetComponent<Text>().text = QuestManager.thisManager.questList.qList[questID].title;
        questInfo.GetChild(1).GetComponent<Text>().text = QuestManager.thisManager.questList.qList[questID].description;
    }
    public void QuestAdd(int questID) {
        print("Creating");
        Transform quest = (Transform)Instantiate(questPrefab, Vector3.zero, Quaternion.identity);
        quest.SetParent(questContent);
        InventoryManager.thisManager.helpArrange(quest);
        quest.GetChild(0).GetComponent<Text>().text = questList.qList[questID].title;
        quest.GetComponent<Button>().onClick.AddListener(() => QuestShow(questID));
        quest.transform.SetAsFirstSibling();
        playerQuests.Add(questList.qList[questID]);
        HUDManager.thisManager.QuestPrompt(questID, 1);
    }
    public void QuestComplete(int questID) {
        for (int i = 0; i < playerQuests.Count; i++) {
            if (playerQuests[i].questID == questID) {
                playerQuestsCompleted.Add(playerQuests[i]);
                playerQuests.RemoveAt(i);
                questContent.GetChild(i).SetAsLastSibling();
                //Change quest status to grayed out or give it a checkmark.
            }
        }
        HUDManager.thisManager.QuestPrompt(questID, 0);
    }
    public bool QuestCheck(int questID) {
        print(playerQuests.Count);
        for (int i = 0; i < playerQuests.Count; i++) {
            print("CheckQuest " + i );
            if(playerQuests[i].questID == questID) {
                return true;
            }
        }
        return false;
    }
}
public class QuestList {
    public List<Quest> qList;
    public QuestList() {
        qList = new List<Quest>();
    }
}
public class Quest {
    public int questID;
    public string title;
    public string description;
    public Quest() {

    }
    public Quest(string _title, string _description) {
        title = _title;
        description = _description;
    }
}
