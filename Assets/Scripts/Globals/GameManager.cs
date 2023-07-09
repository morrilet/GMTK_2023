using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [HideInInspector] public Camera mainCamera;
    [HideInInspector] public CharacterController characterController;

    private int score = 0;

    private void AssignInstance() {
        if (!instance) {
            instance = this;
        } else {
            GameObject.DestroyImmediate(this.gameObject);
        }
        GameObject.DontDestroyOnLoad(this);
    }

    private void Awake() {
        AssignInstance();
    }

    public void AssignCamera(Camera cam) {
        AssignInstance();  // Just in case this is called before Awake() by some other class.
        mainCamera = cam;
    }

    public void AssignCharacterController(CharacterController controller) {
        AssignInstance();  // Just in case this is called before Awake() by some other class.
        characterController = controller;
    }

    public void CollectSnack() {
        score++;
    }
}
