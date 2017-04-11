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
    


    void Awake() {
        thisManager = this;
        thisCanvas = this.GetComponent<CanvasGroup>();

        playerHP = GameObject.FindGameObjectWithTag("PlayerHP");
        bossHP = GameObject.FindGameObjectWithTag("BossHP");
        playerHPCanvas = playerHP.GetComponent<CanvasGroup>();
        bossHPCanvas = bossHP.GetComponent<CanvasGroup>();

        OOCvar = StartCoroutine(OOC());
    }
    public void UpdateHealth(int health) {
        if(OOCvar != null) {
            StopCoroutine(OOCvar);
            OOCvar = StartCoroutine(OOC());
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
    public void UpdateObjective(Quest quest) {
        
    }
    public void DisableHud() {
        StartCoroutine(Fade(thisCanvas));
    }
    IEnumerator Fade(CanvasGroup Canvas) {
        while (Canvas.alpha != 0) {
            yield return new WaitForSeconds(fadeTime);
            Canvas.alpha -= fadeRate;
        }
    }
    IEnumerator OOC() {
        yield return new WaitForSeconds(OOCTime);
        StartCoroutine(Fade(playerHPCanvas));

    }
}
