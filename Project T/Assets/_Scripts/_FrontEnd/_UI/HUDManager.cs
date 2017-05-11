 using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class HUDManager : MonoBehaviour {
    public static HUDManager thisManager;
    CanvasGroup thisCanvas;
    //CanvasGroup playerHPCanvas;
    //CanvasGroup bossHPCanvas;

    //GameObject playerHP;
    //GameObject bossHP;
    [Space(10)]
    public List<Image> st = new List<Image>();
    public float staminaPoints = 100f;

    public List<Image> hp = new List<Image>();
    public float healthPoints = 100f;
    public CanvasGroup playerHPCanvas;

    public List<Image> ma = new List<Image>();
    public float magicaPoints = 100f;

    public List<Image> bhp = new List<Image>();
    public float bossHealthPoints = 100f;
    public CanvasGroup bossHPCanvas;
    [Space(10)]

    public float fadeRate;
    public float fadeTime;
    [Space(10)]

    public float oOCTime;
    Coroutine oOCvar;
    [Space(10)]

    public GameObject questCompleted;
    public GameObject questGained;

    void Awake()
    {
        thisManager = this;
        thisCanvas = GetComponent<CanvasGroup>();
        #region deprecated
        //playerHP = GameObject.FindGameObjectWithTag("PlayerHP");
        //bossHP = GameObject.FindGameObjectWithTag("BossHP");

        //playerHPCanvas = playerHP.GetComponent<CanvasGroup>();
        //bossHPCanvas = bossHP.GetComponent<CanvasGroup>();
        //bossHPCanvas.alpha = 0;
        //OOCvar = StartCoroutine(FadeTimer(OOCTime,playerHP.transform));
        #endregion
        bossHPCanvas.alpha = 0;
    }
    /*public void UpdateHealth(int health)
    {
        if(OOCvar != null)
        {
            StopCoroutine(OOCvar);
            //OOCvar = StartCoroutine(FadeTimer(OOCTime,playerHP.transform)); 
        }
        playerHPCanvas.alpha = 1;
        //playerHP.GetComponent<Image>().fillAmount = health / 100;

    }*/
    public void HealthUpdate(float healthPointsUpdate)
    {
        healthPoints = healthPointsUpdate;

        foreach (Image health in hp)
        {
            health.fillAmount = healthPoints / 100;
        }

       /* if(healthPoints<= 100 && playerHPCanvas.alpha <= 1)
        {
            bossHPCanvas.alpha = 1;
        }*/
    }

    public void BossHealthUpdate(float healthPointsAddition)
    {
        foreach (Image health in bhp)
        {
            health.fillAmount = healthPoints / 100;
        }

        if (healthPoints <= 100 && bossHPCanvas.alpha != 0)
        {
            bossHPCanvas.alpha = 1;
        }
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
