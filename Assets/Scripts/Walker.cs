using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Walker : MonoBehaviour
{
    NavMeshAgent agent;
    [SerializeField]NodeMap nodeMap;
    int currentNode = 0;
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        agent.destination = nodeMap.nodes[currentNode].position;

        if (!agent.hasPath || agent.velocity.sqrMagnitude == 0f){
            if(currentNode == nodeMap.nodes.Length -1) {
                currentNode = 0;
            } else {
                currentNode++;
            }
        }
    }
}
