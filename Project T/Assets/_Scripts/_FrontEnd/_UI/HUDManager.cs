 using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class HUDManager : MonoBehaviour {
    public static HUDManager thisManager;
    CanvasGroup thisCanvas;
    CanvasGroup playerHPCanvas;
    CanvasGroup bossHPCanvas;

    GameObject playerHP;
    GameObject bossHP;

    public float fadeRate;
    public float fadeSpeed;
    public float fadeWait;
    public bool fading;
    List<int> fadeQueue = new List<int>();

    public float OOCTime;
    Coroutine OOCvar;

    public GameObject questObj;

    public GameObject itObject;

    List<int> fadingList = new List<int>();

    void Awake() {
        thisManager = this;
        thisCanvas = this.GetComponent<CanvasGroup>();

        questObj = GameObject.FindGameObjectWithTag("questObj");

        playerHP = GameObject.FindGameObjectWithTag("PlayerHP");
        bossHP = GameObject.FindGameObjectWithTag("BossHP");

        itObject = GameObject.FindGameObjectWithTag("itObject");

        playerHPCanvas = playerHP.GetComponent<CanvasGroup>();
        bossHPCanvas = bossHP.GetComponent<CanvasGroup>();
        bossHPCanvas.alpha = 0;
        //OOCvar = StartCoroutine(FadeTimer(OOCTime,playerHP.transform));

    }
    public void Interaction(bool toggle, int spriteID) {
        if (toggle) {
            print("/Sprites/Interaction" + spriteID);
            itObject.GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/Interaction" + spriteID);
            itObject.SetActive(true);
        }
        else {
            itObject.SetActive(false);
        }
    }
    public void UpdateHealth(float health) {
        if(OOCvar != null) {
            StopCoroutine(OOCvar);
            //OOCvar = StartCoroutine(FadeTimer(OOCTime,playerHP.transform)); 
        }
        playerHPCanvas.alpha = 1;
        playerHP.GetComponent<Image>().fillAmount = health / 100;

    }
    public void UpdateBossHealth(int health) {
        if (bossHPCanvas.alpha != 0) {
            bossHPCanvas.alpha = 1;
        }
        bossHP.GetComponent<Image>().fillAmount = health / 100;

    }
    public void QuestPrompt(int questID, int option) {
        if(option == 0) {
            StartCoroutine(Fade(questObj.GetComponent<CanvasGroup>()));
            questObj.GetComponent<Text>().text = "Quest Completed";
            questObj.transform.GetChild(0).GetComponent<Text>().text = QuestManager.thisManager.questList.qList[questID].title;
        }
        else {
            StartCoroutine(Fade(questObj.GetComponent<CanvasGroup>()));
            questObj.transform.GetChild(0).GetComponent<Text>().text = QuestManager.thisManager.questList.qList[questID].title;
            questObj.GetComponent<Text>().text = "New Quest";
        }
    }
    IEnumerator Fade(CanvasGroup Canvas) {
        fading = true;
        while(Canvas.alpha < 0.95) {
            print("fading in");
            Canvas.alpha = Mathf.Lerp(Canvas.alpha, 1, fadeRate);
            yield return new WaitForSeconds(fadeSpeed);
        }
        print("Fadewait began");
        yield return new WaitForSeconds(fadeWait);
        print("Fadewait up");
        while (Canvas.alpha > 0.1) {
            Canvas.alpha = Mathf.Lerp(Canvas.alpha, 0, fadeRate);
            yield return new WaitForSeconds(fadeSpeed);
        }
        Canvas.alpha = 0;
        if(fadeQueue.Count > 0) {
            
        }
        yield break;
    }



    ////Fade functionality
    ////Fades given canvas group out
    //IEnumerator FadeOut(CanvasGroup Canvas) {
    //    while (Canvas.alpha != 0) {
    //        yield return new WaitForSeconds(fadeTime);
    //        Canvas.alpha -= fadeRate;
    //    }
    //}
    ////Fades given canvas group in
    //IEnumerator FadeIn(CanvasGroup Canvas) {
    //    fading = true;
    //    while (Canvas.alpha != 1) {
    //        yield return new WaitForSeconds(fadeTime);
    //        Canvas.alpha += fadeRate;
    //    }
        
    //}
    ////Time before it fades something out
    //IEnumerator FadeTimer(float time,Transform fader) {
    //    yield return new WaitForSeconds(time);
    //    StartCoroutine(FadeOut(fader.GetComponent<CanvasGroup>()));

    //}
}
