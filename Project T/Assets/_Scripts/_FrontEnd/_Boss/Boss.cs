using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour {
    UnityEngine.AI.NavMeshAgent agent;
    public Transform i;
    Transform player;

	void Start () {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        //StartCoroutine(Turn());
    }
    void Update() {

    }
    IEnumerator Turn() {
        yield return new WaitForSeconds(0);
    }
}
