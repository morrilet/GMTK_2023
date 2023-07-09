using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EndTrigger : MonoBehaviour
{
    [SerializeField]GameObject canvasObj;
    [SerializeField]TMP_Text scoreText;
    [SerializeField]LayerMask playerLayer; // [PLACEHOLDER

    // Start is called before the first frame update
    void OnTriggerEnter(Collider col) {
        if (col.gameObject.layer == 7) {
            Debug.Log("Collided");
            if (GameManager.instance.characterController.walkerNodeFollower.LastNode()) {
                canvasObj.SetActive(true);
                scoreText.text = $"You managed to gobble {GameManager.instance.score} pieces of toast! Wow!";
            }
        }
    }
}
