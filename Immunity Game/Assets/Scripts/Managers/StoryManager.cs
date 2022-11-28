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
    public bool currentSceneIsSkipScene;

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
                //nextScene();
                loadWonScene();
                return;
            }

            if (Input.GetKey(KeyCode.Z) || noMoreCells()) // Needs to be deleted upon final arrival
            {
                loadLostScene();
            }
            return;
        }
        
        if (currentSceneIsSkipScene)
        {
            if(Input.anyKey)
            {
                loadMainMenu();
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
    
    private void loadMainMenu()
    {
        SceneManager.LoadScene(0);
        print("Scene was changed to Main Menu: ");
    }
    
    private void loadWonScene()
    {
        SceneManager.LoadScene("Scenes/Other/Level_Won");
        print("Scene was changed to Main Menu: ");
    }
    
    private void loadLostScene()
    {
        SceneManager.LoadScene("Scenes/Other/Level_Lost");
        print("Scene was changed to Main Menu: ");
    }
    
    private bool noMoreBacteria()
    {
        List<GameObject> bacteriaSmall = BacteriaSmallManager.Instance.pooledBacterias;
        List<GameObject> bacteriaLarge = BacteriaLargeManager.Instance.pooledBacterias;

        GameObject[] bacteria = bacteriaLarge.Concat(bacteriaSmall).ToArray();
            
        if (!bacteria.Any() )
        {
            print("YOU WON!");
            return true;
        }
        return false;
    }
    
    private bool noMoreCells()
    {
        List<GameObject> cells = CellManager.Instance.cellList;
        List<GameObject> bacteriaLarge = BacteriaLargeManager.Instance.pooledBacterias;

        GameObject[] cell = cells.ToArray();
            
        if (!cell.Any() )
        {
            print("YOU LOST!");
            return true;
        }
        return false;
    }
}
