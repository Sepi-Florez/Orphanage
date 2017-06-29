using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundTrigger : MonoBehaviour
{
    public SoundManager soundManager;

    public void FootStep(string randomizeSounds)
    {
        string[] sounds = randomizeSounds.Split('/');
        soundManager.SoundLister(UnityEngine.Random.Range(int.Parse(sounds[0]), int.Parse(sounds[1])));
    }

}