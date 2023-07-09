using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireHydrant : MonoBehaviour, IInteractable
{
    public GameObject emitters;
    [SerializeField] float hydrantTime = 8;
    float hydrantTimer = 0;
    
    void Start() {
        emitters.SetActive(false);
    }

    void Update() {
        hydrantTimer += Time.deltaTime;

        if(emitters.activeSelf && hydrantTimer >= hydrantTime){
            emitters.SetActive(false);
        }
    }

    public void Interact(InteractableTrigger obj) {
        if(!emitters.activeSelf){
            emitters.SetActive(true);
            AudioManager.PlayOneShot(GlobalVariables.SFX_FIRE_HYDRANT);
            hydrantTimer = 0;
        }
    }
}
