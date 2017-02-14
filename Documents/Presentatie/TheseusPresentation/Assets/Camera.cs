using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera : MonoBehaviour {
    public float h;
    public float r;
    public float speedR;
    public float speedH;
    bool move;
    void Update() {
        Input_();
    }
    void Input_() {
        if(Input.GetButtonDown("Fire1")) {
            if (!move) {
                StartCoroutine(NextSlide());
            }

        }
    }
    IEnumerator NextSlide() {
        float rotatedAmount = 0f;
        float decendAmount = 0f;
        move = false;
        while (decendAmount <= h || rotatedAmount <= r) {
            if (rotatedAmount <= r) {
                float addedValue = speedR * Time.deltaTime;
                rotatedAmount += addedValue;
                transform.parent.transform.Rotate(new Vector3(0,-addedValue,0) * speedR * Time.deltaTime);

                //rotate with addedValue
            }
            if (decendAmount <= h) {
                float addedValue = speedH * Time.deltaTime;
                decendAmount += addedValue;
                transform.parent.transform.Translate(new Vector3(0, -addedValue, 0) * speedH * Time.deltaTime);
            }

            yield return null;
        }
    }
}
