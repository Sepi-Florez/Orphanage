using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine.UI;
using UnityEngine;
using System.Xml;
using System.IO;
using System;

[System.Serializable]
public class OptionList
{

    [XmlArray("OptionList")]
    public List<Option> optionList;

    public OptionList()
    {
        optionList = new List<Option>();
    }

    public void AddOption(Option toAdd)
    {
        optionList.Add(toAdd);
    }

    public List<string> GetOptionIDs()
    {
        List<string> toReturn = new List<string>();
        foreach (Option opt in optionList)
        {
            toReturn.Add(opt.id);
        }
        return toReturn;
    }

    public string GetOptionText(string oName)
    {
        foreach (Option opt in optionList)
        {
            if (oName == opt.id)
            {
                return opt.text;
            }
        }
        return null;
    }

    /*public string GetOption(string oName)
    {
        foreach (Option opt in optionList)
        {
            if (oName == opt.id)
            {
                return opt.text;
            }
        }
        return null;
    }*/

}

[Serializable]
public class Option
{
    [XmlAttribute("ID")]
    public string id;

    [XmlElement("Text")]
    public string text;

    [XmlElement("LinksTo")]
    public List<string> linksTo = new List<string>();

    [XmlElement("LinkText")]
    public List<string> linkText = new List<string>();
}

public class DialSystem : MonoBehaviour
{
    public string path = "/Dialogue.xml"; //gets parsed from NPC
    [Space(10)]
    public Button templateButton;
    public RectTransform contentWindow;
    [Space(10)]
    public OptionList options;

    List<Button> currentOptions = new List<Button>();

    public OptionList Load(string filePath)
    {
        FileStream stream = new FileStream(filePath, FileMode.Open);
        XmlSerializer reader = new XmlSerializer(typeof(OptionList));
        OptionList list = reader.Deserialize(stream) as OptionList;
        stream.Close();

        return list;
    }

    void Start()
    {
        options = Load(Application.dataPath + path);
        Generate(0);
        //print(options.optionList[0].text);
    }


    bool Enabled()
    {
        return true;
    }

    void Generate(int o)
    {
        if (currentOptions.Count != 0)
        {
            for (int i = 0; i <= currentOptions.Count - 1;)
            {
                Destroy(currentOptions[i]);
                //Should really destroy listeners to minimize performance loss
            }
            currentOptions.Clear();
        }

        Option nxtOpt = options.optionList[o];

        for(int i = 0; i <= nxtOpt.linkText.Count-1; i++)
        {
            Button button = Instantiate(templateButton, Vector3.zero, Quaternion.identity);
            //print("Beta " + (int.Parse(nxtOpt.linksTo[i])) + " Count " + i);
            button.GetComponentInChildren<Text>().text = nxtOpt.linkText[i];
            button.onClick.AddListener(() => Generate(int.Parse(nxtOpt.linksTo[i])));
            button.transform.SetParent(contentWindow);
            currentOptions.Add(button);
        }
    }

    #region Can be checked by QuestSystem
    void CheckForQuest()
    {

    }

    void CheckForItem()
    {

    }
    #endregion
}