using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rope : MonoBehaviour
{
    public Transform anchor_1;
    public Transform anchor_2;

    public float resolution = 0.5f;
    public float width = 0.1f;

    protected LineRenderer lineRenderer;
    protected List<Transform> vertices;

    private void Awake() {
        lineRenderer = GetComponent<LineRenderer>();
    }

    protected void InitializeRope() {
        int numVertices = (int)Mathf.Ceil(Vector3.Distance(anchor_1.position, anchor_2.position) / resolution);
        vertices = new List<Transform>();

        for (int i = 0; i < numVertices; i++) {
            GameObject newVertex = new GameObject("Rope Vertex");
            Rigidbody newRigidbody = newVertex.AddComponent<Rigidbody>();
            CapsuleCollider newCollider = newVertex.AddComponent<CapsuleCollider>();
            HingeJoint newJoint = newVertex.AddComponent<HingeJoint>();

            // Parent later rope segments to the previous one, but the first to the main object.
            if (i == 0) {
                newVertex.transform.parent = transform;
                newJoint.connectedBody = anchor_1.GetComponent<Rigidbody>();
            }
            else {
                newVertex.transform.parent = vertices[i - 1].transform;
                newJoint.connectedBody = vertices[i - 1].GetComponent<Rigidbody>();
            }

            newVertex.transform.position = Vector3.Lerp(anchor_1.position, anchor_2.position, (float)i / ((float)numVertices - 1));
            newVertex.transform.rotation = Quaternion.identity;
            newVertex.transform.localScale = Vector3.one;
            vertices.Add(newVertex.transform);
        }
    }

    private void Start() {
        InitializeRope();

        lineRenderer.startWidth = width;
        lineRenderer.endWidth = width;
        lineRenderer.positionCount = vertices.Count;
    }

    private void FixedUpdate() {
        for (int i = 0; i < vertices.Count; i++) {
            lineRenderer.SetPosition(i, vertices[i].position);
        }
    }
}
