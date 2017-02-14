using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera : MonoBehaviour {
    public float h;
    public float r;
    public float speed;
    public float t = 0;
    public float tt;
    void Update() {
        Input_();
    }
    void Input_() {
        if(Input.GetButtonDown("Fire1")) {
            t = 0;
            StartCoroutine(NextFrame());

        }
    }
    IEnumerator NextFrame() {
        t++;
        if (t == tt) {

        }

            yield return new WaitForSeconds(speed);

    }
}
