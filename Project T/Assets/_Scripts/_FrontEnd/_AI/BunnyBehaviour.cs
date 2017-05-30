using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

//Serializable]
public class BunnyBehaviour : MonoBehaviour {

    public bool chaseMode = false;
    public GameObject[] bunnyHoles;
    float distance = Mathf.Infinity;
    Coroutine corry;
    List<Collider> playerChecker = new List<Collider>();

    public Transform target;
    NavMeshAgent agent;
    float waitTime;
    bool inWaitState = false;

    public float minWaitTime;
    public float maxWaitTime;

    public float range = 10.0f;
    bool randomPoint(Vector3 center, float range, out Vector3 result) {
        for (int i = 0; i < 2; i++) {
            Vector3 randomPoint = center + Random.insideUnitSphere * range;
            print("RandomPoint" + "is" + randomPoint);
            NavMeshHit hit;
            if (NavMesh.SamplePosition(randomPoint, out hit, range, NavMesh.AllAreas)) {
                print("NavMeshHit" + hit + hit.position);
                result = hit.position;
                return true;
            }
        }
        result = Vector3.zero;
        print("error");
        //StartCoroutine(BunnyMoment());
        return false;
    }

    // Use this for initialization
    void Start() {
        agent = GetComponent<NavMeshAgent>();
        //agent.SetDestination(target.position);
        //corry = StartCoroutine(BunnyMovement());
        NormalMoveTrig();
    }



    // Update is called once per frame
    /*void Update() {
   
            print("Fire1Clicked");
            currentMoveState();
    }*/

    public void PlayerHasEntered() {
        ChaseChecker();
    }

    void ChaseChecker() {
        distance = Mathf.Infinity;
        StartCoroutine(BunnyChaseBehaviour());
    }

    void NormalMoveTrig() {
        if (chaseMode == false) {
            print("chaseMode=False");
            if (agent.remainingDistance <= agent.stoppingDistance) {
                print("RemainingDistance<=StoppingDistance");
                print("CurrentWait" + inWaitState);
                if (inWaitState == false) {
                    print("WaitState=False");
                    StartCoroutine(BunnyMovement());
                    print("CoroutineStarted");
                }
            }
        }
    }

    IEnumerator BunnyChaseBehaviour() {
        print("chaseMode Is" + chaseMode);
        foreach(GameObject bunnyHole in bunnyHoles) {
            //print("bunnyHole" + bunnyHole);
            Vector3 diff = bunnyHole.transform.position - transform.position;
            float curDistance = diff.sqrMagnitude;
            if (curDistance < distance) {
                GameObject closest = bunnyHole;
                distance = curDistance;
                agent.SetDestination(bunnyHole.transform.position);
                print("BunnyHole" + bunnyHole + "Is Closest to Rabbit");
                print("ChaseWentThrough");
            }
            chaseMode = false;
            yield return distance;
        }
        //agent.SetDestination(target.position);
    }

    IEnumerator BunnyMovement() {
        Vector3 point;
        print("CoroutineStartedMovement");
        while (randomPoint(transform.position, range, out point)) {
            print("StartOfWhile");
            print(point);
            inWaitState = true;
            waitTime = Random.Range(minWaitTime, maxWaitTime);
            print(waitTime);
            yield return new WaitForSeconds(waitTime);
            target.position = point;
            agent.SetDestination(target.position);
            Debug.DrawRay(point, Vector3.up, Color.blue, 1.0f);
            inWaitState = false;
            print(agent.remainingDistance);
            print("EndOfWhile");
        }
        print("EndOfMove");
        yield return point;
        //StopCoroutine(BunnyMovement());
    }
}

