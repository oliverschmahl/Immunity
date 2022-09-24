using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameMenu : MonoBehaviour
{
    public string mainMenu, firstLevel, secondLevel, thirdLevel, fourthLevel, fithLevel;
    public GameObject optionsScreen, menuScreen;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
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