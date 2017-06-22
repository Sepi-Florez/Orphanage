using UnityEngine;

public class HealthManager
{
    public static HealthManager thisManager;
    public HUDManager hud;

    void Awake()
    {
        thisManager = this;
    }

    public static float healthPoints = 100f;

    //public static AudioClip damageSound;

    public static AudioSource effectsAudioSource;

    public static void UpdateHP(float hpAddition)//,AudioClip sound)
    {
        healthPoints += hpAddition;
        HUDManager.thisManager.UpdateHealth(healthPoints);
        if (hpAddition <= 0)
        {
            //effectsAudioSource.clip = sound;
            //fectsAudioSource.Play();
        }
    }
}