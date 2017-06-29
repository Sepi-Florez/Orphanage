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

    public string name;

    public Text answer;
    public List<Text> responses = new List<Text>();

    DialogueNode currentNode;

    public List<int> questIds = new List<int>();
    public List<int> recipeIds = new List<int>();
    public List<int> itemIds = new List<int>();
    public List<int> itemCount = new List<int>();

    bool end = false;

    public string dataPath;
    [SerializeField]
    public XmlDocument pi;

    private void Awake() {
        Load();
        //currentNode = Deserialize(data);
    }
    private void Update() {
        if (end) {
            if(Input.GetButtonDown("Cancel") || Input.GetButtonDown("Jump")) {
                EndConversation();
            }
        }
    }
    private void Load() {


        /*print(Application.persistentDataPath + dataPath);
        if (File.Exists(Application.dataPath + dataPath)) {
            FileStream stream = new FileStream(Application.dataPath + dataPath, FileMode.Open);
            XmlSerializer reader = new XmlSerializer(typeof(DialogueNode));
            currentNode = reader.Deserialize(stream) as DialogueNode;
            stream.Close();
        }*/
        TextAsset n = (TextAsset)Resources.Load(dataPath);
        if (n != null) {
            currentNode = Deserialize(n);
            print("Deserialized!");
        }
        else {
            print("Conversation file not found");
        }
    }
    DialogueNode Deserialize(TextAsset xmlFile) { 
        XmlSerializer serializer = new XmlSerializer(typeof(DialogueNode));
        using (System.IO.StringReader reader = new System.IO.StringReader(xmlFile.text))
        {
        return serializer.Deserialize(reader) as DialogueNode;

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
        canvas.GetChild(0).GetChild(1).GetComponent<Text>().text = name;
        responses.Clear();
        foreach (Transform response in canvas.GetChild(1)) {
            responses.Add(response.GetChild(0).GetComponent<Text>());
            response.GetComponent<Button>().onClick.AddListener(() => UpdateConversation(response.GetSiblingIndex()));
        }
    }
    //Starts the conversation
    public void StartConversation() {
        MakeCanvas();
        Interaction.thisManager.it = false;
        HUDManager.thisManager.Toggle(0);
        CursorLock.Lock();
        if(SlabManager.thisManager.slabOpen == true) {
            SlabManager.thisManager.SlabToggle();

        }
        SlabManager.thisManager.toggling = false;
        UpdateConversation(-1);
        //disable stuff
    }
    //end the conversation
    public void EndConversation() {
        HUDManager.thisManager.Toggle(1);
        SlabManager.thisManager.toggling = true;
        Destroy(canvas.gameObject);
        CursorLock.Lock();
        Interaction.thisManager.it = true;
        ExtraDo();
        Load();
        end = false;
        
        //enable stuff again
    }
    //closes the conversation for a part. Will still show the npc answer.
    public void CloseConversation() {
        end = true;
        foreach(Text i in responses) {
            Destroy(i.transform.parent.gameObject);
        }
        answer.text = currentNode.answer;
    }
    // Will based upon the response replace the answer of the npc and the responses for the player.
    public void UpdateConversation(int i) {

        if (i != -1) {
            currentNode = currentNode.responseNodes[i];
            ExtraCheck();
            if (currentNode.end) {
                CloseConversation();
                return;
            }
        }
        answer.text = currentNode.answer;
        for (int ii = 0; ii < responses.Count; ii++) {
            responses[ii].text = currentNode.responses[ii];

        }

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
            print("Adding recipe");
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
