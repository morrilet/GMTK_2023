using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableTrigger : MonoBehaviour
{
    GameObject nearbyInteractable = null;
    bool buttonPrev = false;

    void Update()
    {
        if(nearbyInteractable != null) {
            if(Input.GetAxis("Interact") == 1 && !buttonPrev){
                nearbyInteractable.GetComponent<IInteractable>().Interact();
                AudioManager.PlayOneShot(GlobalVariables.SFX_UI_CLICK);
                buttonPrev = true;
            }
        }
        if(Input.GetAxis("Interact") != 1){
            buttonPrev = false;
        }
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
