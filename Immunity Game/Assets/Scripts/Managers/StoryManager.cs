using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Managers;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StoryManager : MonoBehaviour
{
    private int ns;
    public bool currentSceneIsALevel;

    // Update is called once per frame
    void Update()
    {
        if (currentSceneIsALevel)
        {
            //Need to change the check to make sure that it handles dead bacteria correctly
            //A Valid suggestion is the below comment:
            //if (BacteriaLargeManager.Instance == null && BacteriaSmallManager.Instance == null)
            if(Input.GetKey(KeyCode.X) || noMoreBacteria()) // Needs to be deleted upon final arrival
            {
                nextScene();
            }
            return;
        }
        
        if (Input.anyKey)
        {
            nextScene();
        }
    }

    private void nextScene()
    {
        ns = SceneManager.GetActiveScene().buildIndex + 1;
        SceneManager.LoadScene(ns);
        print("Scene was changed to scene index: " + ns);
    }
    
    private bool noMoreBacteria()
    {
        List<GameObject> bacteriaSmall = BacteriaSmallManager.Instance.pooledBacterias;
        List<GameObject> bacteriaLarge = BacteriaLargeManager.Instance.pooledBacterias;

        GameObject[] bacteria = bacteriaLarge.Concat(bacteriaSmall).ToArray();
            
        //print(bacteria.Length);
            
        if (bacteria.Length < 1)
        {
            print("YOU WON!");
            return true;
        }
        return false;
    }
}
