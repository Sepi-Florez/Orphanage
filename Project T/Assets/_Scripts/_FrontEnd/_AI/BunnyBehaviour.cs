using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BunnyBehaviour : MonoBehaviour {

    public bool chaseMode = false;
    public GameObject[] bunnyHoles;
    float distance = Mathf.Infinity;
    Coroutine corry;
    List<Collider> playerChecker = new List<Collider>();
    public float chaseSpeed;
    MeshRenderer bunnyMesh;

    public Transform target;
    NavMeshAgent agent;
    float waitTime;
    bool inWaitState = false;

    public float minWaitTime;
    public float maxWaitTime;

    public float range = 10.0f;
    //Hieronder wordt een randompoint in een bepaalde radius vanaf de bunny op de navmesh gekozen.
    bool randomPoint(Vector3 center, float range, out Vector3 result) {
        for (int i = 0; i < 30; i++) {
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
        return false;
    }

    // Wordt gebruit om de normalmovement te triggeren en de NavMesh component aan te geven.
    void Start() {
        agent = GetComponent<NavMeshAgent>();
        NormalMoveTrig();
        bunnyMesh = transform.GetComponent<MeshRenderer>();
    }



    //Wordt aangeroepen door het script PlayerChecker als een object met de layer "Player" in zijn collider komt. Start de "ChaseFase" van de rabbit.
    public void PlayerHasEntered() {
        distance = Mathf.Infinity;
        StartCoroutine(BunnyChaseBehaviour());
    }

    //Normale Movement van de Rabbit, checked eerst of hij in zijn "NormalState" zit en stuurt hem dan aan.
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
    //De IEnumerator die alles voor de bunny chase regelt.
    IEnumerator BunnyChaseBehaviour() {
        print("chaseMode Is" + chaseMode);
        foreach(GameObject bunnyHole in bunnyHoles) {
            Vector3 diff = bunnyHole.transform.position - transform.position;
            float curDistance = diff.sqrMagnitude;
            if (curDistance < distance) {
                GameObject closest = bunnyHole;
                distance = curDistance;
                agent.SetDestination(bunnyHole.transform.position);
                if(agent.stoppingDistance == agent.remainingDistance) {
                    print("Yellow");
                    bunnyMesh.enabled = false;
                }
                print("BunnyHole" + bunnyHole + "Is Closest to Rabbit");
                print("ChaseWentThrough");
            }
            yield return distance;
        }
    }
    //De IEnumerator die alles voor de bunny movement regelt.
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
        NormalMoveTrig();
    }
}

