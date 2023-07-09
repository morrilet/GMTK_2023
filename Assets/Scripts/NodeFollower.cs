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

        if (nodeMap != null)
            agent.destination = nodeMap.nodes[currentNode].position;
    }

    public virtual void ReselectCurrentNode() {
        if (nodeMap == null)
            return;

        agent.destination = nodeMap.nodes[currentNode].position;
    }

    public virtual void NextNode() {
        if (nodeMap == null)
            return;

        if(currentNode == nodeMap.nodes.Length -1) {
            currentNode = 0;
        } else {
            currentNode++;
        }
        agent.destination = nodeMap.nodes[currentNode].position;
    }

    protected virtual void Update() {
        if (Vector3.Distance(transform.position, agent.destination) < 1.0f){
            NextNode();
        }
    }

    public bool LastNode() {
        return currentNode == nodeMap.nodes.Length - 1;
    }

    // public void OnDrawGizmos() {
    //     if (nodeMap == null && agent == null)
    //         return;

    //     Gizmos.color = Color.yellow;
    //     Gizmos.DrawLine(transform.position, nodeMap.nodes[currentNode].position);

    //     Gizmos.color = Color.red;
    //     Gizmos.DrawLine(transform.position, agent.destination);
    // }
}
