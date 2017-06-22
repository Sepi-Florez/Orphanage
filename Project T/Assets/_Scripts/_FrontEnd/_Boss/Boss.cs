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
    //Health
    public int health;
    private int currentHealth;

    //floats
    public float minAngle;
    public float chargeSpeed;
    public float maxChargeSpeed;
    public float weaponRadius;

    //Bools
    public bool looking = true;
    public bool lookFollow;
    public bool charge = false;

    //Coroutines
    public Coroutine looker;

    //Fills in some variables
	void Start () {

        player = GameObject.FindGameObjectWithTag("Player").transform;
        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        anim = GetComponent<Animator>();
        currentHealth = health;
        agent.isStopped = true;
        //Damage(10);
        //StartLooking();
        
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
            if(r == 0) {
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
                transform.rotation = Quaternion.RotateTowards(transform.rotation, rotation, 2);
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
            RaycastHit hit;
            if (Physics.Raycast(transform.position,transform.forward,out hit, 3)) {
                if(hit.transform.tag == "Player") {
                    charge = false;
                    HealthManager.UpdateHP(-10);
                    print("Charge Hit Player");
                }
                charge = false;
                anim.SetBool("Charge", false);
                break;
            }
            chargeSpeed += maxChargeSpeed / 100;
            if (chargeSpeed > maxChargeSpeed)
                chargeSpeed = maxChargeSpeed;
            transform.position = Vector3.MoveTowards(transform.position, lastPos, chargeSpeed * Time.deltaTime);
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
    }
    void Sweep() {
        anim.SetTrigger("Sweep");
        bossAction -= Sweep;

    }
    void BullCharge() {
        anim.SetTrigger("Charge");
        lookFollow = true;
        StartCoroutine(AttackFollow());
        bossAction -= BullCharge;
    }
    // A function to be called through animation events
    void EventCall(int i) {
        lookFollow = false;
        switch (i) {
            case 1:
                charge = true;
                StartCoroutine(Charge());
                break;
        }

    }
    // A check which checks if theres a player within the position of the weapon
    void HitCheck() {
        print("checking hit");
        RaycastHit hit;
        Collider[] list = Physics.OverlapSphere(weapon.position, weaponRadius, hitLayer);
        foreach(Collider col in list) {
            if(col.transform.tag == "Player") {
                print("Player hit");
                HealthManager.UpdateHP(-30);
                //col.attachedRigidbody.
            }
        }
    }
    void Damage(int damage) {
        currentHealth -= damage;
        HUDManager.thisManager.UpdateBossHealth(currentHealth);
    }
}
