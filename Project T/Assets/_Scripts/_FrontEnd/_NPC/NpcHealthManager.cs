using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcHealthManager : MonoBehaviour
{
    public float healthPoints = 100f;

    public AudioClip damageSound;

    public AudioSource effectsAudioSource;
    
    public void Start()
    {
        effectsAudioSource = GetComponent<AudioSource>();
    }

    public void UpdateHP(float hpAddition)
    {
        healthPoints += hpAddition;
        print("I'm a person and I'm being hurt");
        if (hpAddition <= 0)
        {
            effectsAudioSource.clip = damageSound;
            effectsAudioSource.Play();
        }
    }
}
