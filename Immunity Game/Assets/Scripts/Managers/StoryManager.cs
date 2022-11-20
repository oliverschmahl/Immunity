using System;
using System.Collections;
using System.Collections.Generic;
using Managers;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StoryManager : MonoBehaviour
{
    private int nextScene;
    public bool currentSceneIsALevel;

    // Update is called once per frame
    void Update()
    {
        if (currentSceneIsALevel)
        {
            //Need to change the check to make sure that it handles dead bacteria correctly
            //A Valid suggestion is the below comment:
            //if (BacteriaLargeManager.Instance == null && BacteriaSmallManager.Instance == null)
            if(Input.GetKey(KeyCode.X)) // Needs to be deleted upon final arrival
            {
                nextScene = SceneManager.GetActiveScene().buildIndex + 1;
                SceneManager.LoadScene(nextScene);
                print("Scene was changed to scene index: " + nextScene);
            }
            return;
        }
        
        if (Input.anyKey)
        {
            nextScene = SceneManager.GetActiveScene().buildIndex + 1;
            SceneManager.LoadScene(nextScene);
            print("Scene was changed to scene index: " + nextScene);
        }
    }
}
