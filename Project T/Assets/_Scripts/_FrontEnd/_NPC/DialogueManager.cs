using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System;
using System.Xml;
using System.Xml.Serialization;
public class DialogueManager : MonoBehaviour {
    public Transform ConversationCanvasPrefab;
    Transform canvas;

    public Text answer;
    public List<Text> responses = new List<Text>();
    public string[] test;

    DialogueNode currentNode;

    public List<int> questIds = new List<int>();
    public List<int> recipeIds = new List<int>();
    public List<int> itemIds = new List<int>();
    public List<int> itemCount = new List<int>();

    bool end = false;

    public string dataPath;

    private void Awake() {
        Load();
    }
    private void Update() {
        if (end) {
            if(Input.GetButtonDown("Cancel") || Input.GetButtonDown("Jump")) {
                EndConversation();
            }
        }
    }
    private void Load() {
        if (File.Exists(Application.dataPath + dataPath)) {
            FileStream stream = new FileStream(Application.dataPath + dataPath, FileMode.Open);
            XmlSerializer reader = new XmlSerializer(typeof(DialogueNode));
            currentNode = reader.Deserialize(stream) as DialogueNode;
            stream.Close();
        }
        else {
            print("Conversation file not found");
        }
    }
    /*public void Start() {
        currentNode = new DialogueNode(answer.text, test, 3);
        Test();
    }*/
    //Can be called to start conversation.
    public void MakeCanvas() {
        canvas = (Transform)Instantiate(ConversationCanvasPrefab, Vector3.zero, Quaternion.identity);
        answer = canvas.GetChild(0).GetChild(0).GetComponent<Text>();
        responses.Clear();
        foreach (Transform response in canvas.GetChild(1)) {
            responses.Add(response.GetChild(0).GetComponent<Text>());
            response.GetComponent<Button>().onClick.AddListener(() => UpdateConversation(response.GetSiblingIndex()));
        }
    }
    //Starts the conversation
    public void StartConversation() {
        MakeCanvas();
        if(SlabManager.thisManager.slabOpen == true) {
            SlabManager.thisManager.SlabToggle();

        }
        SlabManager.thisManager.toggling = false;
        UpdateConversation(-1);
        //disable stuff
    }
    //Ends the conversation
    public void EndConversation() {
        SlabManager.thisManager.toggling = true;
        Destroy(canvas.gameObject);
        ExtraDo();
        Load();
        end = false;
        
        //enable stuff again
    }

    public void CloseConversation() {
        end = true;
        foreach(Text i in responses) {
            Destroy(i.transform.parent.gameObject);
        }
        answer.text = currentNode.answer;
    }
    // Will based upon the response replace the answer of the npc and the responses for the player.
    public void UpdateConversation(int i) {
        if(i != -1) {
            currentNode = currentNode.responseNodes[i];
            if (currentNode.end) {
                CloseConversation();
                return;
            }
        }
        answer.text = currentNode.answer;
        for (int ii = 0; ii < responses.Count; ii++) {
            responses[ii].text = currentNode.responses[ii];

        }
        ExtraCheck();
    }
    //This function checks if there are Quest,Items or Recipes to be added at the end of the conversation.
    public void ExtraCheck() {
        if (currentNode.questID != 0) {
            questIds.Add(currentNode.questID);
        }
        if (currentNode.recipeID != 0) {
            recipeIds.Add(currentNode.recipeID);
        }
        if (currentNode.itemID != 0) {
            itemIds.Add(currentNode.itemID);
            itemCount.Add(currentNode.itemCount);
        }
    }
    //Activates the corresponding functions
    public void ExtraDo() {
        foreach(int i in questIds) {
            QuestManager.thisManager.QuestAdd(i);
        }
        for(int i = 0; i < itemIds.Count; i++) {
            InventoryManager.thisManager.InventoryAdd(itemIds[i], itemCount[i]);
        }
        foreach(int i in recipeIds) {
            CraftingManager.thisManager.AddRecipe(i);
        }
    }
    //A function only used to write the initial xml file. 
    public void Test() {
        XmlSerializer writer = new XmlSerializer(typeof(DialogueNode));
        FileStream stream = new FileStream(Application.dataPath + dataPath, FileMode.Create);
        print(Application.persistentDataPath + dataPath);
        writer.Serialize(stream, currentNode);
        stream.Close();
    }
}
public class DialogueNode {
    public string answer;
    public string[] responses;
    public DialogueNode[] responseNodes;
    public bool end;
    public int questID;
    public int recipeID;
    public int itemID;
    public int itemCount;

    public DialogueNode(string _answer, string[] _responses, int diaOptions) {
        answer = _answer;
        responses = _responses;
        if (diaOptions != 0) {
            responseNodes = new DialogueNode[3];
            for (int i = 0; i < responseNodes.Length; i++) {
                responseNodes[i] = new DialogueNode(answer, responses, diaOptions - 1);
            }
        }
        else {
            end = true;
        }
    }
    DialogueNode() {

    }
}
