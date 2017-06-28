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
    List<int> fadeQueueC = new List<int>();

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
    void Start() {
    }

    public void Toggle(int i) {
        thisCanvas.alpha = i;
    }
    public void Interaction(bool toggle, int spriteID) {
        if (toggle) {
            //print("/Sprites/Interaction" + spriteID);
            itObject.GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/Interaction" + spriteID);
            itObject.GetComponent<CanvasGroup>().alpha = 1;
        }
        else {
            itObject.GetComponent<CanvasGroup>().alpha = 0;
        }
    }
    public void UpdateHealth(float health) {
        //print("health update : " + health);
        playerHP.transform.GetChild(1).GetComponent<Image>().fillAmount = health / 100;
        playerHPCanvas.alpha = 1;
        if (OOCvar == null) {
            OOCvar = StartCoroutine(OOC());
        }
        else {
            StopCoroutine(OOCvar);
            StartCoroutine(OOC());
        }



    }
    public void UpdateBossHealth(float health) {
        if (bossHPCanvas.alpha == 0) {
            bossHPCanvas.alpha = 1;
        }
        print(health);
        bossHP.transform.GetChild(1).GetComponent<Image>().fillAmount = health / 100;
        if (health <= 0) {
            StartCoroutine(Fade(bossHPCanvas));
        }

    }
    public void QuestPrompt(int questID, int option) {
        if (fading) {
            fadeQueue.Add(questID);
            fadeQueueC.Add(option);
        }
        else {
            if (option == 0) {
                StartCoroutine(Fade(questObj.GetComponent<CanvasGroup>()));
                questObj.transform.GetChild(0).GetComponent<Text>().text = "Quest Completed";
                questObj.transform.GetChild(1).GetComponent<Text>().text = QuestManager.thisManager.questList.qList[questID].title;
            }
            else {
                StartCoroutine(Fade(questObj.GetComponent<CanvasGroup>()));
                questObj.transform.GetChild(1).GetComponent<Text>().text = QuestManager.thisManager.questList.qList[questID].title;
                questObj.transform.GetChild(0).GetComponent<Text>().text = "New Quest";
            }
        }
    }
    IEnumerator Fade(CanvasGroup Canvas) {
        fading = true;
        while (Canvas.alpha < 0.95) {
            //print("fading in");
            Canvas.alpha = Mathf.Lerp(Canvas.alpha, 1, fadeRate);
            yield return new WaitForSeconds(fadeSpeed);
        }
        Canvas.alpha = 1;
        //print("Fadewait began");
        yield return new WaitForSeconds(fadeWait);
        //print("Fadewait up");
        while (Canvas.alpha > 0.05) {
            Canvas.alpha = Mathf.Lerp(Canvas.alpha, 0, fadeRate);
            yield return new WaitForSeconds(fadeSpeed);
        }
        Canvas.alpha = 0;
        fading = false;
        if (fadeQueue.Count > 0) {
            int i = fadeQueue[0];
            int ii = fadeQueueC[0];
            fadeQueue.RemoveAt(0);
            fadeQueueC.RemoveAt(0);
            QuestPrompt(i, ii);

        }
        yield break;
    }
    IEnumerator OOC() {
        yield return new WaitForSeconds(OOCTime);
        while (playerHPCanvas.alpha > 0.05) {
            playerHPCanvas.alpha = Mathf.Lerp(playerHPCanvas.alpha, 0, fadeRate);
            yield return new WaitForSeconds(fadeSpeed);
        }
        playerHPCanvas.alpha = 0;
    }
}