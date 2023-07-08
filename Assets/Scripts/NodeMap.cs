using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeMap : MonoBehaviour
{
    [HideInInspector] public Transform[] nodes;

    private void Awake() {
        nodes = GetComponentsInChildren<Transform>();
    }
}
