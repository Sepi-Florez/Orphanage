using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthManager : MonoBehaviour
{
    public static HealthManager thisManager;
    public HUDManager hud;

    void Awake()
    {
        thisManager = this;
    }

    public float healthPoints = 100f;
    //public float staminaPoints = 100f;
    //public float magicaPoints = 100f;

    public AudioClip damageSound;

    public AudioSource effectsAudioSource;

    public void UpdateHP(float hpAddition)
    {
        healthPoints += hpAddition;
        if (hud != null)
        {
            hud.HealthUpdate(healthPoints);
        }

        if (hpAddition <= 0)
        {
            effectsAudioSource.clip = damageSound;
            effectsAudioSource.Play();
        }
    }
    /*public void UpdateST(float stAddition)
    {
        staminaPoints += stAddition;
    }
    public void UpdateMA(float maAddition)
    {
        magicaPoints += maAddition;
    }*/
}