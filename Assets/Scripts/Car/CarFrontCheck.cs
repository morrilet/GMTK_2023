using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarFrontCheck : MonoBehaviour
{
    int cols = 0;

    [HideInInspector] public bool colliding = false;

    void OnTriggerEnter(Collider col) {
        cols ++;
    }

    void OnTriggerExit(Collider col) {
        cols --;
    }

    void Update() {
        if (cols == 0) {
            colliding = false;
        } else {
            colliding = true;
        }
    }
}
