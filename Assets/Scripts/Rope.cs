using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rope : MonoBehaviour
{
    public Transform anchor_1;
    public Transform anchor_2;

    public float resolution = 0.5f;
    public float width = 0.1f;
    public float damper = 0.25f;

    protected LineRenderer lineRenderer;
    protected List<Transform> vertices;

    private void Awake() {
        lineRenderer = GetComponent<LineRenderer>();
    }

    protected void InitializeRope() {
        int numVertices = (int)Mathf.Ceil(Vector3.Distance(anchor_1.position, anchor_2.position) / resolution);
        vertices = new List<Transform>();

        // Set up the vertex objects.
        for (int i = 0; i < numVertices; i++) {
            GameObject newVertex = new GameObject("Rope Vertex");

            // Parent later rope segments to the previous one, but the first to the main object.
            if (i == 0) {
                newVertex.transform.parent = transform;
            }
            else {
                newVertex.transform.parent = vertices[i - 1].transform;
            }

            newVertex.transform.position = Vector3.Lerp(anchor_1.position, anchor_2.position, (float)i / ((float)numVertices - 1));
            // newVertex.transform.rotation = Quaternion.LookRotation(anchor_2.position - anchor_1.position);
            newVertex.transform.localScale = Vector3.one;
            vertices.Add(newVertex.transform);
        }

        // Add the hinges once everything is in place.
        for (int i = 0; i < numVertices; i++) {
            GameObject newVertex = vertices[i].gameObject;
            CapsuleCollider newCollider = newVertex.AddComponent<CapsuleCollider>();
            Rigidbody newRigidbody = newVertex.AddComponent<Rigidbody>();
            HingeJoint newJoint = newVertex.AddComponent<HingeJoint>();

            if (i == 0) {
                newRigidbody.isKinematic = true;
            } else {
                newJoint.connectedBody = vertices[i - 1].GetComponent<Rigidbody>();
            }
            
            newCollider.radius = width * 0.5f;
            newCollider.height = resolution;
            newCollider.direction = 2;  // Y-axis

            newJoint.anchor = new Vector3(0f, 0f, resolution * -0.5f);

            JointSpring newSpring = newJoint.spring;
            newSpring.damper = damper;
            newJoint.spring = newSpring;
            newJoint.useSpring = true;
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
