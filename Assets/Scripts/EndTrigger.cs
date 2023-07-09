using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EndTrigger : MonoBehaviour
{
    [SerializeField]GameObject canvasObj;
    [SerializeField]TMP_Text scoreText;
    // Start is called before the first frame update
    void OnTriggerEnter(Collider col) {
        if (col.gameObject.layer == 7) {
            canvasObj.SetActive(true);
            scoreText.text = "You managed to gobble [PLACEHOLDER] pieces of toast! Wow!";
        }
    }
}
