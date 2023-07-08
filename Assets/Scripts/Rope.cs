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
            else if (i == numVertices - 1) {
                newVertex.transform.parent = transform;
                newVertex.transform.parent = vertices[i - 1].transform;
            }
            else {
                newVertex.transform.parent = vertices[i - 1].transform;
            }

            newVertex.transform.position = Vector3.Lerp(anchor_1.position, anchor_2.position, (float)i / ((float)numVertices - 1));
            newVertex.transform.rotation = Quaternion.LookRotation(anchor_2.position - anchor_1.position);
            newVertex.transform.localScale = Vector3.one;
            newVertex.layer = LayerMask.NameToLayer("Rope");
            vertices.Add(newVertex.transform);
        }

        // Add the hinges once everything is in place.
        for (int i = 0; i < numVertices; i++) {
            GameObject newVertex = vertices[i].gameObject;
            CapsuleCollider newCollider = newVertex.AddComponent<CapsuleCollider>();
            Rigidbody newRigidbody = newVertex.AddComponent<Rigidbody>();
            ConfigurableJoint newJoint = newVertex.AddComponent<ConfigurableJoint>();

            newJoint.anchor = new Vector3(0f, 0f, resolution * -0.5f);
            newJoint.enablePreprocessing = false;
            newJoint.xMotion = ConfigurableJointMotion.Locked;
            newJoint.yMotion = ConfigurableJointMotion.Locked;
            newJoint.zMotion = ConfigurableJointMotion.Locked;
            newJoint.angularXMotion = ConfigurableJointMotion.Limited;
            newJoint.angularYMotion = ConfigurableJointMotion.Limited;
            newJoint.angularZMotion = ConfigurableJointMotion.Locked;
            newJoint.projectionMode = JointProjectionMode.PositionAndRotation;
            newJoint.projectionAngle = 180f;
            newJoint.projectionDistance = 0.1f;

            if (i == 0) {
                newRigidbody.isKinematic = true;
                GameObject.Destroy(newCollider);
            } 
            else if (i == numVertices - 1) {
                newRigidbody.isKinematic = true;
                newJoint.connectedBody = vertices[i - 1].GetComponent<Rigidbody>();
                GameObject.Destroy(newCollider);
            } else {
                newJoint.connectedBody = vertices[i - 1].GetComponent<Rigidbody>();
            }
            
            newCollider.radius = width * 0.5f;
            newCollider.height = resolution;
            newCollider.direction = 2;  // Y-axis

            // JointSpring newSpring = newJoint.spring;
            // newSpring.damper = damper;
            // newJoint.spring = newSpring;
            // newJoint.useSpring = true;
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

        vertices[0].position = anchor_1.position;
        vertices[vertices.Count - 1].position = anchor_2.position;
    }
}
