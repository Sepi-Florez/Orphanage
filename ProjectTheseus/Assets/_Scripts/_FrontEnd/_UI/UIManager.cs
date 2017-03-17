using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {
    public static UIManager thisManager;

    Animator tabletAnim;
    GameObject healthObj;
    GameObject bossObj;
    GameObject ObjectiveObj;
    bool hpOpen;
    bool bhpOpen;
    
    public float oocTime;

    bool tablet = false;

    Coroutine Oocc;
	// Use this for initialization
    void Awake () {
        thisManager = this;
    }
	void Start () {

        tabletAnim = GameObject.FindGameObjectWithTag("Tablet").GetComponent<Animator>();
        healthObj = GameObject.FindGameObjectWithTag("Health");
        bossObj = GameObject.FindGameObjectWithTag("BossHP");
        ObjectiveObj = GameObject.FindGameObjectWithTag("CurrentObjective");
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetButtonDown("Jump")) {
            ToggleTablet();
            BossHealthUpdate("Minotaur",10);
            }
	}
    void ToggleTablet () {
        tablet = !tablet;
        tabletAnim.SetBool("Open", tablet);
    }
    public void HealthUpdate (float health) {
        healthObj.GetComponent<Image>().fillAmount = health / 100;
        healthObj.GetComponent<Animator>().SetBool("Open", true);
        if (Oocc != null) {
            StopCoroutine(Oocc);
        }
        Oocc = StartCoroutine(Ooc());


    }
    public void BossHealthUpdate(string name, float bhp) {
        if (bossObj.transform.GetChild(0).transform.GetComponent<Text>().text == "BossHealth") {
            bossObj.transform.GetChild(0).transform.GetComponent<Text>().text = name;
            bossObj.GetComponent<Animator>().SetBool("Open", true);
        }
        bossObj.transform.GetComponent<Image>().fillAmount = bhp / 100;
    }
    IEnumerator Ooc() {
        yield return new WaitForSeconds(oocTime);
        healthObj.GetComponent<Animator>().SetBool("Open", false);
        yield return null;

    }
    public void CCObjective(string objective) {
        ObjectiveObj.transform.GetChild(0).GetComponent<Text>().text = objective;
    }

}
