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
    public float fadeTime;

    public float OOCTime;
    Coroutine OOCvar;

    public GameObject questCompleted;
    public GameObject questGained;

    void Awake() {
        thisManager = this;
        thisCanvas = this.GetComponent<CanvasGroup>();

        playerHP = GameObject.FindGameObjectWithTag("PlayerHP");
        bossHP = GameObject.FindGameObjectWithTag("BossHP");

        playerHPCanvas = playerHP.GetComponent<CanvasGroup>();
        bossHPCanvas = bossHP.GetComponent<CanvasGroup>();
        bossHPCanvas.alpha = 0;
        OOCvar = StartCoroutine(FadeTimer(OOCTime,playerHP.transform));
    }
    public void UpdateHealth(int health) {
        if(OOCvar != null) {
            StopCoroutine(OOCvar);
            OOCvar = StartCoroutine(FadeTimer(OOCTime,playerHP.transform)); 
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
    public void QuestCompleted(int questID) {
        StartCoroutine(FadeIn(questCompleted.GetComponent<CanvasGroup>()));
        questCompleted.transform.GetChild(0).GetComponent<Text>().text = QuestManager.thisManager.questList.qList[questID].title;
        StartCoroutine(FadeTimer(5,questCompleted.transform));
    }
    public void QuestGained(int questID) {
        StartCoroutine(FadeIn(questGained.GetComponent<CanvasGroup>()));
        questGained.transform.GetChild(0).GetComponent<Text>().text = QuestManager.thisManager.questList.qList[questID].title;
        StartCoroutine(FadeTimer(5,questGained.transform));
    }
    public void DisableHud() {
        StartCoroutine(FadeOut(thisCanvas));
    }
    IEnumerator FadeOut(CanvasGroup Canvas) {
        while (Canvas.alpha != 0) {
            yield return new WaitForSeconds(fadeTime);
            Canvas.alpha -= fadeRate;
        }
    }
    IEnumerator FadeIn(CanvasGroup Canvas) {
        while (Canvas.alpha != 1) {
            yield return new WaitForSeconds(fadeTime);
            Canvas.alpha += fadeRate;
        }
    }
    IEnumerator FadeTimer(float time,Transform fader) {
        yield return new WaitForSeconds(time);
        StartCoroutine(FadeOut(fader.GetComponent<CanvasGroup>()));

    }
}
