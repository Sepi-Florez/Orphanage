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

    public string dataPath;

    private void Awake() {
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
        foreach (Transform response in canvas.GetChild(1)) {
            responses.Add(response.GetChild(0).GetComponent<Text>());
            response.GetComponent<Button>().onClick.AddListener(() => UpdateConversation(response.GetSiblingIndex()));
        }
    }

    public void StartConversation() {
        MakeCanvas();
        if(SlabManager.thisManager.slabOpen == true) {
            SlabManager.thisManager.SlabToggle();
            SlabManager.thisManager.toggling = false;
        }
        UpdateConversation(-1);
        //disable stuff
    }
    public void EndConversation() {
        SlabManager.thisManager.toggling = true;
        Destroy(canvas.gameObject);
        //enable stuff again
    }
    // Will based upon the response replace the answer of the npc and the responses for the player.
    public void UpdateConversation(int i) {
        if(i != -1) {
            currentNode = currentNode.responseNodes[i];
            if (currentNode.end) {
                EndConversation();
                return;
            }
        }
        answer.text = currentNode.answer;
        for (int ii = 0; ii < responses.Count; ii++) {
            responses[ii].text = currentNode.responses[ii];

        }
    }
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
