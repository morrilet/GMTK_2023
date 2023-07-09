using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeMap : MonoBehaviour
{
    [HideInInspector] public Transform[] nodes;

    private void Awake() {
        nodes = GetNodes();
    }

    private Transform[] GetNodes() {
        Transform[] path = new Transform[this.transform.childCount];
        for (int i = 0; i < this.transform.childCount; i++) {
            path[i] = this.transform.GetChild(i).GetComponent<Transform>();
        }
        return path;
    }

    // Note that this isn't executed in a build - only in editor.
    private void OnDrawGizmos() {
        nodes = GetNodes();

        if (nodes == null)
            return;

        Gizmos.color = Color.green;
        for (int i = 0; i < nodes.Length; i++) {
            Gizmos.DrawSphere(nodes[i].position, 1f);
        }

        for (int i = 0; i < nodes.Length - 1; i++) {
            Gizmos.DrawLine(nodes[i].position, nodes[i + 1].position);
        }
    }
}
