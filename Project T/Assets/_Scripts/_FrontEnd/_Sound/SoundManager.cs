using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour {

    public List<AudioClip> useableSounds = new List<AudioClip>();
    public int audioNumber;
    public AudioSource gOAS;

    void Awake() {
        gOAS = transform.GetComponent<AudioSource>();
        /*foreach (AudioClip a in useableSounds) {
            audioNumber = useableSounds.IndexOf(a);
            print(audioNumber);
            Debug.Log(audioNumber);
        }*/

        // Use this for initialization

        // Use this for initialization

       
    }
    public void SoundLister () {
        print("SoundListerActivated!");
        if (audioNumber <= useableSounds.Count) {
            gOAS.PlayOneShot(useableSounds[audioNumber]);
        }
        else {
            print("SoundRequest out of range! The audioNumber you are requesting is not available");
        }
    }

}
