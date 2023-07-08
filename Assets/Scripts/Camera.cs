using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class Camera : MonoBehaviour
{
    [SerializeField] int min = 60;
    [SerializeField] int max = 90;
    public bool frenzied = false;
    CinemachineVirtualCamera cam;
    // Start is called before the first frame update
    void Start()
    {
        cam = GetComponent<CinemachineVirtualCamera>();
    }

    // Update is called once per frame
    void Update()
    {
        if(frenzied && cam.m_Lens.FieldOfView < max){
            cam.m_Lens.FieldOfView += 1;
        } else if (!frenzied && cam.m_Lens.FieldOfView > min) {
            cam.m_Lens.FieldOfView -= 1;
        }
    }
}
