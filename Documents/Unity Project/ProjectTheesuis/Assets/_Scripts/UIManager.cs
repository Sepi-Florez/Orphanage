using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {
    public Animator anim;
    GameObject healthObj;
    public float health;
    public bool hpOpen;
    public float OocTime;

    bool tablet = false;

    Coroutine Oocc;
	// Use this for initialization
	void Start () {
        anim = GameObject.FindGameObjectWithTag("Tablet").GetComponent<Animator>();
        healthObj = GameObject.FindGameObjectWithTag("Health");
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetButtonDown("Jump")) {
            ToggleTablet();
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
    IEnumerator Ooc() {
        yield return new WaitForSeconds(OocTime);
        healthObj.GetComponent<Animator>().SetBool("Open", false);
        yield return null;

    }

}
