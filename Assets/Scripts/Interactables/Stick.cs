using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stick : MonoBehaviour, IInteractable
{
    public Vector3 heldRotationOffset;
    public GameObject tripTrigger;

    InteractableTrigger carrier;
    GameObject attachPoint;
    Rigidbody localRigidbody;
    Collider localCollider;

    private void Awake() {
        localRigidbody = GetComponent<Rigidbody>();
        localCollider = GetComponent<Collider>();
    }

    public void Interact(InteractableTrigger obj) {
        localRigidbody.isKinematic = true;
        localCollider.enabled = false;
        obj.canInteract = false;
        carrier = obj;
        attachPoint = carrier.attachAnchor;
        tripTrigger.SetActive(false);
    }

    private void Update() {
        if (carrier != null) {
            if (Input.GetButtonDown(GlobalVariables.INPUT_INTERACT)) {
                Drop();
            }
        }
    }

    private void Update() {
        if (carrier != null && attachPoint != null) {
            localRigidbody.position = attachPoint.transform.position;
            localRigidbody.rotation = attachPoint.transform.rotation * Quaternion.Euler(heldRotationOffset);
        }
    }

    private void Drop() {
        carrier.canInteract = true;
        localCollider.enabled = true;
        localRigidbody.isKinematic = false;
        carrier = null;
        attachPoint = null;
        tripTrigger.SetActive(true);
    }
}
