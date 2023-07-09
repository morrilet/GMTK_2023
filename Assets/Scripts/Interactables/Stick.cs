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
    bool canDrop;

    private void Awake() {
        localRigidbody = GetComponent<Rigidbody>();
        localCollider = GetComponent<Collider>();
    }

    public void Interact(InteractableTrigger obj) {
        localRigidbody.isKinematic = true;
        localCollider.enabled = false;
        obj.canInteract = false;
        carrier = obj;
        canDrop = false;
        attachPoint = carrier.attachAnchor;
        tripTrigger.SetActive(false);
        AudioManager.PlayOneShot(GlobalVariables.SFX_PICKUP_STICK);
    }

    private void Update() {
        if (carrier != null && canDrop) {
            if (Input.GetButtonDown(GlobalVariables.INPUT_INTERACT)) {
                Drop();
            }
        }

        if (carrier != null && attachPoint != null) {
            localRigidbody.position = attachPoint.transform.position;
            localRigidbody.rotation = attachPoint.transform.rotation * Quaternion.Euler(heldRotationOffset);
        }

        canDrop = true;
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
