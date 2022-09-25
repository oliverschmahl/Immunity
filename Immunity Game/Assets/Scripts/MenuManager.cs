using System.Collections;
using System.Collections.Generic;
using Managers;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    public GameObject menuScreen;
    
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            menuScreen.SetActive(!menuScreen.activeSelf);
            GameManager.Instance.isPaused = menuScreen.activeSelf;
        }
    }
}
