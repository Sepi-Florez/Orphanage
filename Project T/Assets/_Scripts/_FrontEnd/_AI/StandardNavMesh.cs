using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

// Dit is het script dat de AI laat lopen naar een random gekozen positie. Dit doormiddel van de Random.Range. 
// Hieronder worden vrijwel vanzelfspreken de variabels aangemaakt.
public class StandardNavMesh : MonoBehaviour {

    public GameObject ground;

    float xTerrainMin;
    float xTerrainMax;
    float zTerrainMin;
    float zTerrainMax;
    NavMeshHit hit;

    public GameObject goal;
    GameObject artIntel;
    NavMeshAgent agent;

    //Hieronder definieër ik de variabels. Daarnaast vertel ik ook de NavMeshAgent dat hij naar zijn eerste positie moet en dat de movement IEnumerator moet starten.
    void Start() {
        ground = GameObject.FindGameObjectWithTag("Ground");
        artIntel = GameObject.FindGameObjectWithTag("AI");
        agent = artIntel.GetComponent<NavMeshAgent>();
        xTerrainMin = ground.GetComponent<Renderer>().bounds.min.x;
        xTerrainMax = ground.GetComponent<Renderer>().bounds.max.x;
        zTerrainMin = ground.GetComponent<Renderer>().bounds.min.z;
        zTerrainMax = ground.GetComponent<Renderer>().bounds.max.z;
        agent.SetDestination(goal.transform.position);
        StartCoroutine(movementAI(8));
        print("FirstTimeMove");
    }

    // Dit is de IEnumerator die zowel checked of de AI al bij zijn bestemming is en wat de nieuwe bestemming wordt. Hij roept zichzelf steeds aan om een update te besparen.
    IEnumerator movementAI (int waitingTime) {
        if(agent.remainingDistance < agent.stoppingDistance) {
            print("remainingDistance = < stoppingDistance");
            Vector3 position = new Vector3(Random.Range(xTerrainMin, xTerrainMax),2,Random.Range(zTerrainMin,zTerrainMax));
            // return new WaitForSeconds(waitingTime);
            print(position);
            NavMesh.SamplePosition(position, out hit, 5f, 1);
            print(goal.transform.position + "After SamplePos");
            yield return new WaitForSeconds(waitingTime);
            agent.SetDestination(position);
            StartCoroutine(movementAI(10));
        }
    }


    
}
