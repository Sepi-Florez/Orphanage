using UnityEngine;

public class NpcHealthManager : MonoBehaviour
{
    public float healthPoints = 100f;

    public Animator anim;

    public Boss boss;

    bool ded = false;

    //public AudioClip damageSound;

    //public AudioSource effectsAudioSource;
    
    public void Start()
    {
        if(anim == null)
        {
            anim = GetComponent<Animator>();
        }
        //effectsAudioSource = GetComponent<AudioSource>();
    }

    public void UpdateHP(float hpAddition)
    {
        if (!ded)
        {
            healthPoints += hpAddition;
            if (hpAddition <= 0)
            {
                if (boss != null)
                {
                    boss.currentHealth = Mathf.RoundToInt(healthPoints);
                }
                //effectsAudioSource.clip = damageSound;
                //effectsAudioSource.Play();
                if (healthPoints <= 0)
                {
                    ded = true;
                    anim.SetTrigger("Death"); //anim.SetBool("Death", healthPoints <= 0);
                }
            }
        }
    }
}