using UnityEngine;
using System.Collections.Generic;

public class CombatManager : MonoBehaviour
{
    bool checkHit, doDamage;

    public Animator anim;

    public List<Transform> emitters = new List<Transform>();

    public List<Transform> hitObjects = new List<Transform>();

    public float rayLenght = .2f, coolRate = .5f;

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
        anim.SetBool("Fight", Input.GetButton("Fire1") && Time.time > coolTmr);

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
                    print("hit " + hit.transform.name);
                }

                UnityEngine.Debug.DrawRay(ray.origin, ray.direction * rayLenght, colour, .20f);
            }
        }
    }

    public void CheckHit()
    {
        checkHit = !checkHit;
        hitObjects.Clear();
    }

    public void EndCheck()
    {
        checkHit = false;
    }

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