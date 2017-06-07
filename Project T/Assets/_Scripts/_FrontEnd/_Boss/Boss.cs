using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour {
    UnityEngine.AI.NavMeshAgent agent;
    Animator anim;

    Transform player;

    delegate void Action();
    Action bossAction;

    //Health
    public int healh;
    private int currentHealth;

    //floats
    public float minAngle;

    //Bools
    public bool looking = true; 

    //Fills in some variables
	void Start () {

        player = GameObject.FindGameObjectWithTag("Player").transform;
        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        anim = GetComponent<Animator>();
    }

    //decides on the next course of action.
    void DecideAction() {
        print("Deciding");
        float dist = Vector3.Distance(transform.position, player.position);
        float stopDist;
        if (dist > 30) {
            bossAction += BullCharge;
            stopDist = dist;
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
                print("Player in vision");
                transform.LookAt(ppos);
                Move();
                if (agent.isStopped) {
                    agent.isStopped = false;
                    DecideAction();
                }
                else if (agent.remainingDistance - agent.stoppingDistance <= 0) {
                    bossAction();
                    agent.isStopped = true;
                    looking = false;
                }
            }
            else {
                if (!agent.isStopped)
                    agent.isStopped = true;

                Quaternion rotation = Quaternion.LookRotation(ppos - transform.position);
                transform.rotation = Quaternion.RotateTowards(transform.rotation, rotation, 1);
                print("is turning to");
            }
            yield return new WaitForSeconds(0.01f);
        }
    }
    void StartLooking() {
        looking = true;
        StartCoroutine(LookForPlayer());
        print("Start");
    }
    //IEnumerator AttackFollow() {
    //Moves the boss towards the player.
    void Move() {
        print("moving");
        agent.SetDestination(player.position);
    }
    void Overhead() {
        print("Overhead");
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
        bossAction -= BullCharge;
    }
}
