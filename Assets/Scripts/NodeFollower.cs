using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class NodeFollower : MonoBehaviour
{
    [SerializeField] protected NodeMap nodeMap;

    [HideInInspector] public NavMeshAgent agent;

    protected int currentNode = 0;

    protected virtual void Start() {
        agent = GetComponent<NavMeshAgent>();
        agent.destination = nodeMap.nodes[currentNode].position;
    }

    public virtual void ReselectCurrentNode() {
        agent.destination = nodeMap.nodes[currentNode].position;
    }

    public virtual void NextNode() {
        if(currentNode == nodeMap.nodes.Length -1) {
            currentNode = 0;
        } else {
            currentNode++;
        }
        agent.destination = nodeMap.nodes[currentNode].position;
    }

    protected virtual void Update() {
        if (!agent.hasPath || agent.velocity.sqrMagnitude == 0f){
            NextNode();
        }
    }
}
