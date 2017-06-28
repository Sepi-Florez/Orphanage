using UnityEngine;
using System.Collections.Generic;

public class CombatManager : MonoBehaviour
{
    bool checkHit, doDamage;

    public int damageAmount;

    [Header("Animations & Hitboxes")]
    public Animator anim;

    public List<Transform> emitters = new List<Transform>();

    public List<Transform> hitObjects = new List<Transform>();

    public float rayLenght = .2f, coolRate = .5f, minComboPct = 60;

    float coolTmr;

    bool attacking, hasHit, whoosh;

    public SoundManager soundManager;

    public void Awake()
    {
        CursorLock.SetPlayerScripts(this);
    }

    public void Update()
    {
        anim.SetBool("cdFinished", Time.time > coolTmr); //If current time is above"Cool Timer" the bool "Cool Down FInished" will be set to true in the Animator.
        anim.SetBool("fight", Input.GetButtonDown("Fire1"));
        anim.SetBool("fightAlt", Input.GetButtonDown("Fire2"));
        var curAnim = anim.GetCurrentAnimatorStateInfo(0); //We receive the Current Animation from the Animator.
        anim.SetBool("attacking", ((curAnim.IsName("Slash 1")) || (curAnim.IsName("Slash 0")))); //If either of these animations are the current, we'll be "attacking". making it possible to chain attacks.
        anim.SetBool("afterPercentage", (curAnim.normalizedTime >= (minComboPct / 100))); //if we're at minComboPct % of the current animation (no need to rule out attacks) afterPercentage will be set to true in the Animator enabeling us to chain the attacks

        attacking = ((curAnim.IsName("Slash 1")) || (curAnim.IsName("Slash 0")));

        if (whoosh == false && Input.GetButtonDown("Fire1"))
        {
            soundManager.SoundLister(Random.Range(0,1));
            whoosh = true;
        }

        if(whoosh == false && Input.GetButtonDown("Fire2"))
        {
            soundManager.SoundLister(2);
            whoosh = true;
        }
        //if (attacking) { soundManager.SoundLister(5); }

        if (checkHit)
        {
            coolTmr = Time.time + coolRate;
            foreach (Transform emitter in emitters)
            {
                Color colour = Color.green;

                Ray ray = new Ray(emitter.transform.position, -emitter.transform.right); //* .2f);
                //GetMat.GetMaterial(ray);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit, rayLenght))
                {
                    hitObjects.Add(hit.transform);
                }

                Debug.DrawRay(ray.origin, ray.direction * rayLenght, colour, .20f);
            }
            if (hasHit == false)
            {
                for (int i = 0; i <= hitObjects.Count - 1; i++)
                {
                    if (hitObjects[i].tag == "Enemy")
                    {
                        if (hitObjects[i].GetComponentInChildren<NpcHealthManager>())
                        {
                            NpcHealthManager npcHealth = hitObjects[i].GetComponentInChildren<NpcHealthManager>();
                            npcHealth.UpdateHP(-damageAmount);
                            hasHit = true;
                            break;
                        }
                    }
                    else
                    {
                        //hitObjects[i].
                    }
                }
            }
        }
    }

    public void CheckHit()
    {
        checkHit = !checkHit;
        hitObjects.Clear();
        hasHit = false;
    }

    public void EndCheck()
    {
        checkHit = false;
        hitObjects.Clear();//resets hitObjects so you can do damage again.
        hasHit = false;
        whoosh = false;
    }

    public void DoDamage()
    {
        doDamage = !doDamage;
    }
}