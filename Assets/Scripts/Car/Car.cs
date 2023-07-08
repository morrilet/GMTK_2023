using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Car : MonoBehaviour
{
    Rigidbody rigidbody;
    CarFrontCheck carFrontCheck;
    [HideInInspector] public Transform[] nodeMap;
    [SerializeField] int currentNode = 0;
    bool moving = true;

    [SerializeField] int speed = 5;
    [SerializeField] int rotationSpeed = 12;

    public DecreaseCount decreaseCount;

    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        carFrontCheck = GetComponentInChildren<CarFrontCheck>();
    }

    void Update() {
        moving = !carFrontCheck.colliding;
        
        //ToDo: Exclude non-y rotations
        if(moving) {
            float step = rotationSpeed * speed * Time.deltaTime;
            Quaternion target = Quaternion.LookRotation(nodeMap[currentNode].position - this.transform.position);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, target, step);
        }

        if(Vector3.Distance(transform.position, nodeMap[currentNode].position) < 1) {
            if(currentNode == nodeMap.Length - 1) {
                decreaseCount();
                Destroy(this.gameObject);
            } else {
                currentNode++;
            }
        }
    }

    void FixedUpdate()
    {
        Vector3 movement = gameObject.transform.forward;
        if(moving){
            rigidbody.velocity = movement * speed;
        } else {
            if(rigidbody.velocity.magnitude > .1) {
                rigidbody.velocity *= .95f;
            } else {
                rigidbody.velocity = Vector3.zero;
            }
        }
        //Turn when needed
        //Stop for player and owner?       
    }

    public delegate void DecreaseCount();
}
