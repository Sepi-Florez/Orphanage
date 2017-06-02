using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour {
    UnityEngine.AI.NavMeshAgent agent;

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
        StartCoroutine(LookForPlayer());
    }

    //decides on the next course of action.
    void DecideAction() {
        print("Deciding");
        float dist = Vector3.Distance(transform.position, player.position);
        float stopDist;
        print(dist);
        if (dist > 20) {
            bossAction += BullCharge;
            stopDist = dist;
        }
        else {
            int r = Random.Range(0, 1);
            if(r == 0) {

            }
            else {


            }
        }

    }

// Makes the boss look at the player when in the 
    IEnumerator LookForPlayer() {
        while (looking) {
            Vector3 ppos = new Vector3(player.position.x, transform.position.y, player.position.z);
            if(Vector3.Angle(transform.forward ,ppos - transform.position) < minAngle) {

                transform.LookAt(ppos);
                if (agent.isStopped) {
                    DecideAction();
                    agent.isStopped = false;
                }
                if(agent.velocity == Vector3.zero) {  
                    print("stopped");
                    bossAction();
                    looking = false;
                    break;
                }
                Move();
            }
            else {
                if(!agent.isStopped)
                    agent.isStopped = true;

                Quaternion rotation = Quaternion.LookRotation(ppos - transform.position);
                transform.rotation = Quaternion.RotateTowards(transform.rotation, rotation, 1);
                print("is turning to");
            }
            yield return new WaitForSeconds(0.01f);
        }
        print("hagrid");
    }
    //Moves the boss towards the player.
    void Move() {
        print("moving");
        agent.SetDestination(player.position);
    }
        
    void Overhead() {
        print("Overhead");
    }
    void Sweep() {
        print("Sweep");
    }
    void BullCharge() {
        print("CHAAAAAAAAAAARGE");
        bossAction -= BullCharge;
    }

}
