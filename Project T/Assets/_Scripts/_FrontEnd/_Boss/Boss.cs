using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour {
    UnityEngine.AI.NavMeshAgent agent;
    Animator anim;

    Transform player;
    Vector3 lastPos;

    delegate void Action();
    Action bossAction;

    public Transform weapon;

    public LayerMask hitLayer;
    public LayerMask hitLayer2;

    public Vector3[] knockBackForces; //public List<Vector3> knockBackForces = new List<Vector3>();
    private int kBForce;
    //Health
    public int health;

    [HideInInspector]
    public int currentHealth;

    //floats
    public float minAngle;
    public float chargeSpeed;
    public float chargeRadius;
    public float maxChargeSpeed;
    public float weaponRadius;

    //Bools
    public bool looking = true;
    public bool lookFollow;
    public bool charge = false;
    public bool hitting = false;

    //Coroutines
    public Coroutine looker;

    AnimatorStateInfo curAnim;
    Vector3 oldWeaponPos;

    public GameObject ending;

    //Fills in some variables
    void Start() {

        player = GameObject.FindGameObjectWithTag("Player").transform;
        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        anim = GetComponent<Animator>();
        currentHealth = health;
        agent.isStopped = true;
        curAnim = anim.GetCurrentAnimatorStateInfo(0); //Current Animation
                                                       //Damage(10);
                                                       //StartLooking();

    }
    void Update() //moet eigenlijk in een normale functie geplaatst worden die gecalled word als we attacken maar voor nu is het even zo..
    {
        /*if (oldWeaponPos == Vector3.zero) {
            oldWeaponPos = weapon.position;
        }*/

        if (curAnim.normalizedTime % 0.1f == 0)//als current Animation gedeelt kan worden door 0.1 betekend het dat het op x tiende zit van zijn animation..
        {
            oldWeaponPos = weapon.position;//en word de oldWeaponPosition  geupdate met de huidige position (dit is zodat ik een beter idee van impact krijg)
        }
    }

    //decides on the next course of action.
    void DecideAction() {
        print("Deciding");
        float dist = Vector3.Distance(transform.position, player.position);
        float stopDist;
        if (dist > 17) {
            bossAction += BullCharge;
            stopDist = dist;
            charge = true;
        }
        else {
            int r = Random.Range(0, 2);
            if (r == 0) {
                bossAction += Overhead;
                stopDist = 6;
            }
            else {
                bossAction += Sweep;
                stopDist = 5;
            }
        }
        agent.stoppingDistance = stopDist;
    }
    // Makes the boss look at the player when in the 
    IEnumerator LookForPlayer() {
        while (looking) {

            Vector3 ppos = new Vector3(player.position.x, transform.position.y, player.position.z);
            if (Vector3.Angle(transform.forward, ppos - transform.position) < minAngle) {
                if (agent.isStopped) {
                    agent.isStopped = false;
                    DecideAction();
                }
                else if (agent.remainingDistance - agent.stoppingDistance <= 0) {
                    bossAction();

                    anim.SetBool("Walking", false);
                    agent.isStopped = true;
                    looking = false;
                    break;
                }
                anim.SetBool("Walking", true);
                transform.LookAt(ppos);
                Move();
            }
            else {
                if (!agent.isStopped)
                    agent.isStopped = true;


                Quaternion rotation = Quaternion.LookRotation(ppos - transform.position);
                transform.rotation = Quaternion.RotateTowards(transform.rotation, rotation, 4);
            }
            yield return new WaitForSeconds(0.01f);
        }
    }
    public void StartLooking() {
        looking = true;
        looker = StartCoroutine(LookForPlayer());
    }
    // Follows the player until the attack calls for it to stop
    IEnumerator AttackFollow() {
        while (lookFollow) {
            Vector3 ppos = new Vector3(player.position.x, transform.position.y, player.position.z);
            if (Vector3.Angle(transform.forward, ppos - transform.position) < minAngle) {
                transform.rotation = Quaternion.LookRotation((ppos - transform.position).normalized);
                lastPos = player.position;

            }
            yield return new WaitForSeconds(0.01f);
        }
    }
    //Will charge at the player's last position until it hits the play or terrain.
    IEnumerator Charge() {
        while (charge) {
            Collider[] list = Physics.OverlapSphere(transform.position + transform.forward, chargeRadius, hitLayer);
            foreach (Collider col in list) {
                if (list.Length > 0) {
                    if (col.tag == "Player") {
                        charge = false;
                        col.GetComponent<PlayerControlPhysics>().AddForce(knockBackForces[kBForce]);
                        HealthManager.UpdateHP(-10);
                        print("Charge Hit Player");
                    }
                    charge = false;
                    anim.SetBool("Charge", false);
                    print("Collision with " + col.transform.name);
                }
            }
            /*RaycastHit hit;
            Vector3 ppos = new Vector3(lastPos.x, transform.position.y, lastPos.z);
            if (Physics.Raycast(transform.position, ppos ,out hit, 1000, hitLayer2)) {
                if (hit.distance < 10) {
                    if (hit.transform.tag == "Player") {
                        charge = false;
                        HealthManager.thisManager.UpdateHP(-10);
                        print("Charge Hit Player");
                    }
                    charge = false;
                    anim.SetBool("Charge", false);
                    print(hit.transform.name);
                }
            }*/
            chargeSpeed += maxChargeSpeed / 75;
            if (chargeSpeed > maxChargeSpeed)
                chargeSpeed = maxChargeSpeed;
            transform.position = Vector3.MoveTowards(transform.position, lastPos + transform.forward * 100, chargeSpeed * Time.deltaTime);
            yield return new WaitForEndOfFrame();
        }
    }

    void Move() {
        agent.SetDestination(player.position);
    }
    void Overhead() {
        lookFollow = true;
        StartCoroutine(AttackFollow());
        anim.SetTrigger("Overhead");
        bossAction -= Overhead;
        kBForce = 0;
    }
    void Sweep() {
        anim.SetTrigger("Sweep");
        bossAction -= Sweep;
        kBForce = 1;
    }
    void BullCharge() {
        anim.SetTrigger("Charge");
        lookFollow = true;
        StartCoroutine(AttackFollow());
        bossAction -= BullCharge;
        kBForce = 2;
    }
    // A function to be called through animation events
    void EventCall(int i) {
        lookFollow = false;
        switch (i) {
            case 1:
                charge = true;
                //RaycastHit hit;
                //Vector3 ppos = new Vector3(lastPos.x, transform.position.y, lastPos.z);
                //if (Physics.Raycast(transform.position, ppos, out hit, 1000, hitLayer2)) {
                //    lastPos = hit.point;
                //}
                StartCoroutine(Charge());

                break;
        }

    }
    // A check which checks if theres a player within the position of the weapon
    void HitCheck(int i) {
        if (i == 0) {
            hitting = true;
            StartCoroutine(HitChecks());
        }
        else {
            hitting = false;
        }

    }

    IEnumerator HitChecks() {
        while (hitting) {
            Collider[] list = Physics.OverlapSphere(weapon.position, weaponRadius, hitLayer);
            foreach (Collider col in list) {
                if (col.transform.tag == "Player") {
                    print("Player hit");
                    HealthManager.UpdateHP(-30);
                    //col.GetComponent<PlayerControlPhysics>().AddForce((weapon.position - oldWeaponPos).normalized * 100);//applies a force in the direction of the sweep
                    col.GetComponent<PlayerControlPhysics>().AddForce(knockBackForces[kBForce]);
                    hitting = false;
                }
            }
            yield return new WaitForEndOfFrame();
        }
    }
    public void Damage(int damage) {
        currentHealth -= damage;
        HUDManager.thisManager.UpdateBossHealth(currentHealth);
        if (currentHealth <= 0) {
            StopAllCoroutines();
            GetComponent<SoundManager>().SoundLister(0);
            QuestManager.thisManager.QuestComplete(3);
            anim.SetTrigger("Death");
            Instantiate(ending, transform.position, Quaternion.identity);
        }

    }
}
 