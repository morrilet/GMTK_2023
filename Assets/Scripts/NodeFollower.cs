using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class NodeFollower : MonoBehaviour
{
    [SerializeField] protected NodeMap nodeMap;

    protected NavMeshAgent agent;
    protected int currentNode = 0;

    protected virtual void Start() {
        agent = GetComponent<NavMeshAgent>();
    }

    protected virtual void Update() {
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
