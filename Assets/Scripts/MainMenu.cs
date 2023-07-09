using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField]GameObject mainMenu;
    [SerializeField]GameObject settingsMenu;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartGame() {
        SceneManager.LoadScene(1);
    }

    public void Settings() {
        AudioManager.PlayOneShot(GlobalVariables.SFX_DOG_BARK);
    }

    public void BackToMain() {
        mainMenu.SetActive(true);
        settingsMenu.SetActive(false);
    }

    public void ExitGame() {
        Application.Quit();
    }
}
