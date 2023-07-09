using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireHydrant : MonoBehaviour, IInteractable
{
    GameObject emitters;
    // Start is called before the first frame update
    void Start()
    {
        emitters = transform.GetChild(0).gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Interact() {
        emitters.SetActive(true);
        AudioManager.PlayOneShot(GlobalVariables.SFX_FIRE_HYDRANT);
    }
}
