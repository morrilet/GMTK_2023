using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioDebug : MonoBehaviour
{
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.H)) {
            AudioManager.PlayOneShot(GlobalVariables.SFX_DOG_SNIFF);
        }
        if(Input.GetKeyDown(KeyCode.J)) {
            AudioManager.PlayOneShot(GlobalVariables.SFX_DOG_BARK);
        }
        if(Input.GetKeyDown(KeyCode.K)) {
            AudioManager.PlayOneShot(GlobalVariables.SFX_DOG_WALK_CYCLE);
        }
        if(Input.GetKeyDown(KeyCode.L)) {
            AudioManager.PlayOneShot(GlobalVariables.SFX_OWNER_NO);
        }
    }
}
