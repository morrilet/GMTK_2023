using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TripPlayerTrigger : MonoBehaviour 
{
    public LayerMask playerLayer;
    
    private void OnTriggerEnter(Collider other) {
        if (playerLayer == (playerLayer | (1 << other.gameObject.layer))) {
            CharacterController characterController = other.transform.root.GetComponent<CharacterController>();
            if (characterController != null) {
                characterController.TriggerFrenzy();
            }
        }
    }
}
