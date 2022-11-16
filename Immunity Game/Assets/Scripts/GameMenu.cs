using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameMenu : MonoBehaviour
{
    private string mainMenu = "Scenes/MainMenu";
    public GameObject optionsScreen, menuScreen;

    /*Opens Game Menu*/
    public void OpenMenu()
    {
        menuScreen.SetActive(true);
    }
    
    /*Closes Game Menu*/
    public void CloseMenu()
    {
        menuScreen.SetActive(false);
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