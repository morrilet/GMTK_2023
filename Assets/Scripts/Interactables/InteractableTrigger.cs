using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableTrigger : MonoBehaviour
{
    public GameObject attachAnchor;
    
    GameObject nearbyInteractable = null;

    [HideInInspector] public bool canInteract = true;

    bool canInteractPrev;

    void Update()
    {
        if(nearbyInteractable != null && canInteract && canInteractPrev) {
            if(Input.GetButtonDown(GlobalVariables.INPUT_INTERACT)){
                nearbyInteractable.GetComponent<IInteractable>().Interact(this);
                AudioManager.PlayOneShot(GlobalVariables.SFX_UI_CLICK);
            }
        }

        canInteractPrev = canInteract;
    }

    void OnTriggerEnter(Collider col) {
        if(col.gameObject.tag == "Interactable" && nearbyInteractable == null) {
             nearbyInteractable = col.gameObject;
        }
    }

    void OnTriggerStay(Collider col) {
        if(col.gameObject.tag == "Interactable" && nearbyInteractable == null) {
            nearbyInteractable = col.gameObject;
        }
    }

    void OnTriggerExit(Collider col) {
        if(col.gameObject.tag == "Interactable" && nearbyInteractable == col.gameObject) {
            nearbyInteractable = null;
        }
    }
}
