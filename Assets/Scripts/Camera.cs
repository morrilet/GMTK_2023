using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class Camera : MonoBehaviour
{
    [SerializeField] AnimationCurve breatheCurve;
    [SerializeField] int breathScale = 10;
    float breatheCounter = 0;
    [SerializeField] int zoomSpeed = 60;
    [SerializeField] int min = 60;
    [SerializeField] int max = 90;
    public bool frenzied = false;
    CinemachineFreeLook cam;
    // Start is called before the first frame update
    void Start()
    {
        cam = GetComponent<CinemachineFreeLook>();
        cam.m_Lens.FieldOfView = min;
    }

    // Update is called once per frame
    void Update()
    {
        breatheCounter += Time.deltaTime;

        if(frenzied && cam.m_Lens.FieldOfView < max + (breatheCurve.Evaluate(breatheCounter) * breathScale)){
            cam.m_Lens.FieldOfView += zoomSpeed * Time.deltaTime;
        } else if (frenzied && cam.m_Lens.FieldOfView > max + (breatheCurve.Evaluate(breatheCounter) * breathScale)) {
            cam.m_Lens.FieldOfView -= zoomSpeed * Time.deltaTime;
        } else if (!frenzied && cam.m_Lens.FieldOfView > min) {
            cam.m_Lens.FieldOfView -= zoomSpeed * Time.deltaTime;
        }
    }
}
