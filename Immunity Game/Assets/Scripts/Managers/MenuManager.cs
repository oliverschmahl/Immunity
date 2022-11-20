using System.Collections;
using System.Collections.Generic;
using Managers;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    public GameObject menuScreen;
    public GameObject optionsScreen;
    
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            menuScreen.SetActive(!menuScreen.activeSelf);
            optionsScreen.SetActive(false);
            GameManager.instance.isPaused = menuScreen.activeSelf;
        }
    }
}
