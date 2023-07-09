using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bark : MonoBehaviour
{
    [SerializeField]float barkCooldown = .5f;
    float barkCooldownTimer;
    bool buttonPrev = false;

    // Start is called before the first frame update
    void Start()
    {
        barkCooldownTimer = barkCooldown;
    }

    // Update is called once per frame
    void Update()
    {
        barkCooldownTimer += Time.deltaTime;

        if(Input.GetAxis("Bark") == 1) {
            if(!buttonPrev && barkCooldownTimer > barkCooldown){
                barkCooldownTimer = 0;
                AudioManager.PlayOneShot(GlobalVariables.SFX_DOG_BARK);
            }
            buttonPrev = true;
        } else {
            buttonPrev = false;
        }
    }
}
