using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bark : MonoBehaviour
{
    [SerializeField]float barkCooldown = .5f;
    float barkCooldownTimer;

    // Start is called before the first frame update
    void Start()
    {
        barkCooldownTimer = barkCooldown;
    }

    // Update is called once per frame
    void Update()
    {
        barkCooldownTimer += Time.deltaTime;

        if(Input.GetButtonDown(GlobalVariables.INPUT_BARK)) {
            if(barkCooldownTimer > barkCooldown){
                barkCooldownTimer = 0;
                AudioManager.PlayOneShot(GlobalVariables.SFX_DOG_BARK);
            }
        }
    }
}
