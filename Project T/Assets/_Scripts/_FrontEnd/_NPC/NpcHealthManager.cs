using UnityEngine;

public class NpcHealthManager : MonoBehaviour
{
    public float healthPoints = 100f;

    [Space(10)]
    public GameObject spawnOnDeath;

    [Space(10)]
    public bool animate;
    public Animator anim;

    [Space(10)]
    public Boss boss;

    bool ded = false;

    //public AudioClip damageSound;

    //public AudioSource effectsAudioSource;
    
    public void Start()
    {
        if(animate && anim == null)
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
                    if (spawnOnDeath != null)
                    {
                        Instantiate(spawnOnDeath, transform.position, Quaternion.identity);
                    }
                    anim.SetTrigger("Death"); //anim.SetBool("Death", healthPoints <= 0);
                }
            }
        }
    }
}