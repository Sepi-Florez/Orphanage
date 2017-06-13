using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButterflyWingSpeed : MonoBehaviour {

    public float minSpeed;
    public float maxSpeed;
    private GameObject thisObject;
    private Animator thisAnim;
    public float time;

    void Awake ()
    {
        thisObject = this.gameObject;
        thisAnim = thisObject.GetComponent<Animator>();  
    }
	void Start ()
    {
        StartCoroutine(FlySpeed());
	}
    public IEnumerator FlySpeed ()
    {
        while (thisObject != null)
        {
            thisAnim.speed = Random.Range(minSpeed, maxSpeed);
            yield return new WaitForSeconds(time);

        }
    }
}
