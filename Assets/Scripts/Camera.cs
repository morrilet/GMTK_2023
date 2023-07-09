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
    CinemachineVirtualCamera cam;
    // Start is called before the first frame update

    void Start()
    {
        GameManager.instance.AssignCamera(this);

        cam = GetComponent<CinemachineVirtualCamera>();
        cam.m_Lens.OrthographicSize = min;
    }

    // Update is called once per frame
    void Update()
    {
        frenzied = GameManager.instance.characterController.isFrenzied;

        breatheCounter += Time.deltaTime / 6;

        if(frenzied && cam.m_Lens.OrthographicSize < max + (breatheCurve.Evaluate(breatheCounter) * breathScale)){
            cam.m_Lens.OrthographicSize += zoomSpeed * Time.deltaTime;
        } else if (frenzied && cam.m_Lens.OrthographicSize > max + (breatheCurve.Evaluate(breatheCounter) * breathScale)) {
            cam.m_Lens.OrthographicSize -= zoomSpeed * Time.deltaTime;
        } else if (!frenzied && cam.m_Lens.OrthographicSize > min) {
            cam.m_Lens.OrthographicSize -= zoomSpeed * Time.deltaTime;
        }
    }
}
