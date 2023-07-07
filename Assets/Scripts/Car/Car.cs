using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Car : MonoBehaviour
{
    Rigidbody rigidbody;
    CarFrontCheck carFrontCheck;
    bool moving = true;

    [SerializeField] int speed = 5;

    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        carFrontCheck = GetComponentInChildren<CarFrontCheck>();
    }

    void Update() {
        moving = !carFrontCheck.colliding;
    }

    void FixedUpdate()
    {
        Vector3 movement = gameObject.transform.rotation * Vector3.forward;
        if(moving){
            rigidbody.velocity = movement * speed;
        } else {
            if(rigidbody.velocity.magnitude > .1) {
                rigidbody.velocity *= .95f;
            } else {
                rigidbody.velocity = Vector3.zero;
            }
        }
        //Move car forward
        //Turn when needed
        //Stop for player and owner?       
    }
}
