using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnackPickup : MonoBehaviour
{
    public LayerMask playerLayer;
    public GameObject pickupObject;

    [Space]

    public float floatSpeed = 1.0f;
    public float floatDistance = 1.0f;
    public float spinSpeed = 1.0f;


    private void OnTriggerEnter(Collider other) {
        if (playerLayer == (playerLayer | (1 << other.gameObject.layer))) {
            Pickup();
        }
    }

    private void Pickup() {
        AudioManager.PlayOneShot(GlobalVariables.SFX_UI_CLICK);
        Destroy(gameObject);
    }

    private void FloatPickup() {
        pickupObject.transform.position = new Vector3(
            pickupObject.transform.position.x, 
            pickupObject.transform.position.y + Mathf.Sin(Time.time * floatSpeed) * floatDistance, 
            pickupObject.transform.position.z
        );
        pickupObject.transform.Rotate(Vector3.up, spinSpeed * Time.deltaTime);
    }

    private void Update() {
        FloatPickup();
    }
}
