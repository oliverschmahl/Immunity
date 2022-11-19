using System.Collections;
using System.Collections.Generic;
using Managers;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameMenu : MonoBehaviour
{
    public string mainMenu;
    public GameObject optionsScreen, menuScreen;

    /*Opens Game Menu*/
    public void OpenMenu()
    {
        menuScreen.SetActive(true);
        GameManager.instance.isPaused = menuScreen.activeSelf;
    }
    
    /*Closes Game Menu*/
    public void CloseMenu()
    {
        menuScreen.SetActive(false);
        GameManager.instance.isPaused = menuScreen.activeSelf;
    }
    
    /*Opens the options menu*/
    public void OpenOptions() {
        optionsScreen.SetActive(true);
    }
    
    /*Closes the options menu*/
    public void CloseOptions() {
        optionsScreen.SetActive(false);
    }

    /*Closes the application*/
    public void ExitToMain() {
        SceneManager.LoadScene(mainMenu);
    }
}