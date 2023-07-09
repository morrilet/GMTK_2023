using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioDebug : MonoBehaviour
{
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.H)) {
            AudioManager.PlayOneShot(GlobalVariables.SFX_OWNER_STOP);
        }
        if(Input.GetKeyDown(KeyCode.J)) {
            AudioManager.PlayOneShot(GlobalVariables.SFX_STEP_CONCRETE);
        }
        if(Input.GetKeyDown(KeyCode.K)) {
            AudioManager.PlayOneShot(GlobalVariables.SFX_STEP_GRASS);
        }
        if(Input.GetKeyDown(KeyCode.L)) {
            AudioManager.PlayOneShot(GlobalVariables.SFX_OWNER_FEEDBACK);
        }
    }
}
