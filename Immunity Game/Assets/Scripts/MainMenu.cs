using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public GameObject optionsScreen;

    /*Starts the game by playing the first level scene*/
    public void StartGame()
    {
        SceneManager.LoadScene("Scenes/1 LEVEL/FirstStory");
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
    public void QuitGame() {
        Application.Quit();
        Debug.Log("Quitting");
    }
}
