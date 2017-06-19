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

    //Health
    public int healh;
    private int currentHealth;

    //floats
    public float minAngle;
    public float chargeSpeed;
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
        StartLooking();
    }

    //decides on the next course of action.
    void DecideAction() {
        print("Deciding");
        float dist = Vector3.Distance(transform.position, player.position);
        float stopDist;
        if (dist > 25) {
            bossAction += BullCharge;
            stopDist = dist;
            charge = true;
        }
        else {
            int r = Random.Range(0, 2);
            if(r == 0) {
                bossAction += Overhead;
                stopDist = 7;
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
    void StartLooking() {
        looking = true;
        looker = StartCoroutine(LookForPlayer());
        print("Start looking");
    }
    IEnumerator AttackFollow() {
        while (lookFollow) {
            Vector3 ppos = new Vector3(player.position.x, transform.position.y, player.position.z);
            if (Vector3.Angle(transform.forward, ppos - transform.position) < minAngle) {
                transform.LookAt(player);
                lastPos = player.position;
                
            }
            yield return new WaitForSeconds(0.01f);
        }    
    }
    IEnumerator Charge() {
        while (charge) {
            RaycastHit hit;
            if (Physics.Raycast(transform.position,transform.forward,out hit, 3)) {
                if(hit.transform.tag == "Player") {
                    charge = false;
                }
                charge = false;
                break;
            }
            transform.Translate(player.position * chargeSpeed);
            print(lastPos);
            yield return new WaitForSeconds(0.01f);
        }
    }

    void Move() {
        print("moving");
        agent.SetDestination(player.position);
    }
    void Overhead() {
        print("Overhead");
        lookFollow = true;
        StartCoroutine(AttackFollow());
        anim.SetTrigger("Overhead");
        bossAction -= Overhead;
    }
    void Sweep() {
        print("Sweep");
        anim.SetTrigger("Sweep");
        bossAction -= Sweep;

    }
    void BullCharge() {
        print("CHAAAAAAAAAAARGE");
        anim.SetTrigger("Charge");
        lookFollow = true;
        StartCoroutine(AttackFollow());
        bossAction -= BullCharge;
    }
    void EventCall(int i) {
        lookFollow = false;
        switch (i) {
            case 1:
                charge = true;
                StartCoroutine(Charge());
                break;
        }

    }
    void HitCheck() {
        RaycastHit hit;
        if(Physics.SphereCast(weapon.position,weaponRadius,transform.forward, out hit)) {
            if(hit.transform.tag == "Player") {
                print("Player");
            }
        }
    }
}
