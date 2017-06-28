using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour {

    public List<AudioClip> useableSounds = new List<AudioClip>();
    public AudioSource gOAS;

    public bool playFirst = false;

    void Awake() {
        gOAS = transform.GetComponent<AudioSource>();

    }
    private void Start() {
        if (useableSounds.Count != 0 && playFirst) {
            gOAS.loop = true;
            gOAS.PlayOneShot(useableSounds[0]);
        }
    }
    public void SoundLister (int i) {
        print("SoundListerActivated!");
        if (i <= useableSounds.Count) {
            gOAS.PlayOneShot(useableSounds[i]);
        }
        else {
            print("SoundRequest out of range! The audioNumber you are requesting is not available");
        }
    }

}
