using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public string firstLevel, secondLevel, thirdLevel, fourthLevel, fithLevel;
    public GameObject optionsScreen;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    /*Starts the game by playing the first level scene*/
    public void StartGame()
    {
        SceneManager.LoadScene(firstLevel);
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
