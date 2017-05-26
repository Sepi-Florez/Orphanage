using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour {
    UnityEngine.AI.NavMeshAgent agent;

    Transform player;


    //Health
    public int healh;
    private int currentHealth;

    //floats
    public float minAngle;

    //Bools
    bool looking = true; 

    //Fills in some variables
	void Start () {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        StartCoroutine(LookForPlayer());
    }

    //decides on the next course of action.
    void MakeAction() {

    }

    //Turns the boss towards the player. When the player is in vision he lookat until out of angle.
    IEnumerator LookForPlayer() {
        while (looking) {
            Vector3 ppos = new Vector3(player.position.x, transform.position.y, player.position.z);
            print("looking");
            if(Vector3.Angle(transform.forward ,ppos - transform.position) < minAngle) {
                transform.LookAt(ppos);
                Move();
                print("is look at");
            }
            else {
                Quaternion rotation = Quaternion.LookRotation(ppos - transform.position);
                transform.rotation = Quaternion.RotateTowards(transform.rotation, rotation, 1);
                print("is turning to");
            }
            yield return new WaitForSeconds(0.01f);
        }

    }
    //Moves the boss towards the player.
    void Move() {
        print("moving");
        agent.SetDestination(player.position);
    }
        
    void Overhead() {

    }
    void Sweep() {

    }
    void BullCharge() {

    }

}
