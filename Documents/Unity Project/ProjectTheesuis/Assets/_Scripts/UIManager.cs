using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {
    public Animator anim;
    GameObject healthObj;
    GameObject bossObj;
    public float health;
    public float bossHealth = 100;
    public bool hpOpen;
    public bool bhpOpen;
    
    public float OocTime;

    bool tablet = false;

    Coroutine Oocc;
	// Use this for initialization
	void Start () {
        anim = GameObject.FindGameObjectWithTag("Tablet").GetComponent<Animator>();
        healthObj = GameObject.FindGameObjectWithTag("Health");
        bossObj = GameObject.FindGameObjectWithTag("BossHP");
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetButtonDown("Jump")) {
            ToggleTablet();
            BossHealthUpdate("Minotaur",bossHealth);
            bossHealth -= 10;
            }
	}
    void ToggleTablet () {
        tablet = !tablet;
        anim.SetBool("Open", tablet);
    }
    public void HealthUpdate (int hit) {
        health -= hit;
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
        yield return new WaitForSeconds(OocTime);
        healthObj.GetComponent<Animator>().SetBool("Open", false);
        yield return null;

    }

}
