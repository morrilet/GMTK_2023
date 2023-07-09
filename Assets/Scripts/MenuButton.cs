using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MenuButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField]AnimationCurve hoverCurve;
    [SerializeField]int magnitude = 15;
    float height;
    RectTransform rect;
    float curveCounter = 0;
    Vector2 rectSize;

    bool hover = false;
    // Start is called before the first frame update
    void Start()
    {
        rect = GetComponent<RectTransform>();
        rectSize = rect.sizeDelta;
        height = rect.localPosition.y;
        curveCounter = Random.Range(1, 300) / 100;
    }

    // Update is called once per frame
    void Update()
    {
        if(!hover) {
            rect.sizeDelta = rectSize;
            curveCounter += Time.deltaTime;
            rect.SetLocalPositionAndRotation(new Vector3(rect.localPosition.x, height + (hoverCurve.Evaluate(curveCounter) * magnitude), rect.localPosition.z), rect.localRotation) ;
        } else {
            rect.sizeDelta = new Vector2(rectSize.x * 1.8f, rectSize.y * 1.8f);
        }
    }

    public void OnPointerEnter(PointerEventData data) {
        hover = true;
    }

    public void OnPointerExit(PointerEventData data) {
        hover = false;
    }
}
