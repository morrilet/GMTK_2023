using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Sniffer : MonoBehaviour
{
    public float maxDistance = 50f;

    List<GameObject> snacks;
    List<GameObject> inteactables;

    GameObject nearest;

    void Start() {
        snacks = new List<GameObject>();
        inteactables = new List<GameObject>();

        AudioManager.PlayMusic(GlobalVariables.SFX_DOG_SNIFF);

        for (int i = 0; i < GameObject.FindObjectsOfType<SnackPickup>().Length; i++) {
            snacks.Add(GameObject.FindObjectsOfType<SnackPickup>()[i].gameObject);
        }

        for (int i = 0; i < GameObject.FindObjectsOfType<Stick>().Length; i++) {
            inteactables.Add(GameObject.FindObjectsOfType<Stick>()[i].gameObject);
        }

        for (int i = 0; i < GameObject.FindObjectsOfType<FireHydrant>().Length; i++) {
            inteactables.Add(GameObject.FindObjectsOfType<FireHydrant>()[i].gameObject);
        }
    }

    void Update() {
        snacks = snacks.Where(x => x != null).ToList();
        inteactables = inteactables.Where(x => x != null).ToList();

        GameObject nearestSnack = snacks.OrderBy(x => Vector3.Distance(transform.position, x.transform.position)).FirstOrDefault().gameObject;
        GameObject nearestInteractable = inteactables.OrderBy(x => Vector3.Distance(transform.position, x.transform.position)).FirstOrDefault().gameObject;
        
        float snackDistance = Vector3.Distance(transform.position, nearestSnack.transform.position);
        float interactableDistance = Vector3.Distance(transform.position, nearestInteractable.transform.position);

        if (snackDistance < interactableDistance) {
            nearest = nearestSnack;
        }
        else {
            nearest = nearestInteractable;
        }

        float distance = Vector3.Distance(transform.position, nearest.transform.position);
        float percent = 100.0f - (distance / maxDistance) * 100.0f;
        AudioManager.SetFloat(GlobalVariables.SFX_DOG_SNIFF, "snack", percent);
    }
}
