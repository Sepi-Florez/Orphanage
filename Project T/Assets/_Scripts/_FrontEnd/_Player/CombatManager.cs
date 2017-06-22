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

    public float rayLenght = .2f, coolRate = .5f, minComboPercentage = 60;

    float coolTmr;

    public void Update()
    {
        #region Deprecated
        /*if (checkHit)
        {
            foreach (Transform emitter in emitters)
            {
                Color colour = Color.green;

                Ray ray = new Ray(emitter.transform.position, -emitter.transform.right); //* .2f);
                RaycastHit hit;

                if(Physics.Raycast(ray, out hit, rayLenght))
                {
                    colour = Color.red;
                    print("hit " + hit.transform.name);
                }

                UnityEngine.Debug.DrawRay(ray.origin, ray.direction * rayLenght, colour, .20f);
            }
        }*/
        #endregion
        anim.SetBool("cdFinished", Time.time > coolTmr);
        anim.SetBool("fight", Input.GetButtonDown("Fire1"));
        anim.SetBool("fightAlt", Input.GetButtonDown("Fire2"));
        var curAnim = anim.GetCurrentAnimatorStateInfo(1);
        anim.SetBool("attacking", ((curAnim.IsName("Slash 1")) || (curAnim.IsName("Slash 0"))));    //&& (anim.GetCurrentAnimatorStateInfo(0).length * anim.GetCurrentAnimatorStateInfo(0).speed >= ));
        anim.SetBool("afterPercentage", (curAnim.normalizedTime >= (minComboPercentage / 100)));

        if (checkHit)
        {
            coolTmr = Time.time + coolRate;
            foreach (Transform emitter in emitters)
            {
                Color colour = Color.green;

                Ray ray = new Ray(emitter.transform.position, -emitter.transform.right); //* .2f);
                RaycastHit hit;

                if(Physics.Raycast(ray, out hit, rayLenght))
                {
                    colour = Color.red;
                    hitObjects.Add(hit.transform);
                    //print("hit " + hit.transform.name);
                }

                UnityEngine.Debug.DrawRay(ray.origin, ray.direction * rayLenght, colour, .20f);
            }
            for(int i = 0; i <= hitObjects.Count-1; i++)
            {
                if(hitObjects[i].tag == "NPC")
                {
                    if (hitObjects[i].GetComponentInChildren<NpcHealthManager>())
                    {
                        NpcHealthManager npcHealth = hitObjects[i].GetComponentInChildren<NpcHealthManager>();
                        npcHealth.UpdateHP(damageAmount);
                        break;
                    }
                }
            }
        }
    }

    public void CheckHit()
    {
        checkHit = !checkHit;
        hitObjects.Clear();
    }

    /*public void EndCheck()
    {
        checkHit = false;
    }*/

    public void DoDamage()
    {
        doDamage = !doDamage;
    }

    #region Test
    /*
    public void OnDrawGizmos()
    {

    }
    */

    /*
    int caseSwitch = 1;
    switch (caseSwitch)
    {
        case 1:
            Console.WriteLine("Case 1");
            break;
        case 2:
            Console.WriteLine("Case 2");
            break;
        default:
            Console.WriteLine("Default case");
            break;
    }
    */

    /*
     var div : int = animation["attack"].normalizedTime + 0;
    var progression = (animation["attack"].wrapMode == WrapMode.Loop) ? animation[anim].normalizedTime - div : animation["attack"].normalizedTime;

    if( progression < 0.3 ) didHit = false; // Make sure we hit just once
    if( progression > 0.5 && !didHit ) Hit();
     */
    #endregion
}