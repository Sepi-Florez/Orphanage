using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BunnyBehaviour : MonoBehaviour {

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
            NavMeshHit hit;
            if (NavMesh.SamplePosition(randomPoint, out hit, 1.0f, NavMesh.AllAreas)) {
                result = hit.position;
                return true;
            }
        }
        result = Vector3.zero;
        //StartCoroutine(BunnyMoment());
        return false;
    }

    // Use this for initialization
    void Start() {
        agent = GetComponent<NavMeshAgent>();
        agent.SetDestination(target.position);
    }

    // Update is called once per frame
    void Update() {
        if (agent.remainingDistance <= agent.stoppingDistance) {
            if (inWaitState == false) {
                StartCoroutine(BunnyMoment());
            }
        }
    }

    IEnumerator BunnyMoment() {
        Vector3 point;
        if (randomPoint(transform.position, range, out point)) {
            print(point);
            inWaitState = true;
            waitTime = Random.Range(minWaitTime, maxWaitTime);
            print(waitTime);
            yield return new WaitForSeconds(waitTime);
            target.position = point;
            agent.SetDestination(target.position);
            Debug.DrawRay(point, Vector3.up, Color.blue, 1.0f);
            inWaitState = false;
        }
        yield return point;
    }
}

