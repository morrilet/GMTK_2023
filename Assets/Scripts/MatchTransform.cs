using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MatchTransform : MonoBehaviour
{
    public Transform target;
    public Vector3 offset;

    private Rigidbody localRigidbody;
    private Transform localTransform;
    
    private void Awake() {
        localRigidbody = GetComponent<Rigidbody>();
        localTransform = GetComponent<Transform>();
    }

    private void FixedUpdate() {
        if (localRigidbody != null) {
            localRigidbody.position = target.position + offset;
        } else {
            localTransform.position = target.position + offset;
        }
    }
}
