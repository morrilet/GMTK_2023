using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireHydrant : MonoBehaviour, IInteractable
{
    [SerializeField] float hydrantTime = 8;
    float hydrantTimer = 0;

    GameObject emitters;
    // Start is called before the first frame update
    void Start()
    {
        emitters = transform.GetChild(0).gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        hydrantTimer += Time.deltaTime;

        if(emitters.activeSelf && hydrantTimer >= hydrantTime){
            emitters.SetActive(false);
        }
    }

    public void Interact() {
        if(!emitters.activeSelf){
            emitters.SetActive(true);
            AudioManager.PlayOneShot(GlobalVariables.SFX_FIRE_HYDRANT);
            hydrantTimer = 0;
        }
    }
}
