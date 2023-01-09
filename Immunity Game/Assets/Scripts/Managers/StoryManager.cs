using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Managers;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

public class StoryManager : MonoBehaviour
{
    public bool currentSceneIsALevel;
    public bool nextSceneShouldBeMainMenu;
    public BacteriaSpawner bacteriaSpawner;

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
                int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
                nextScene(currentSceneIndex);
                print("BUTTON X PRESSED");
                return;
            }

            if (Input.GetKey(KeyCode.Z) || noMoreCells()) // Needs to be deleted upon final arrival
            {
                loadLostScene();
                print("BUTTON Z PRESSED");
            }
            return;
        }
        
        if (nextSceneShouldBeMainMenu)
        {
            if(Input.anyKey)
            {
                loadMainMenu();
            }
            return;
        }
        
        if (Input.anyKey)
        {
            int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
            nextScene(currentSceneIndex);
        }
    }

    private void nextScene(int currentSceneIndex)
    {
        int nsi = currentSceneIndex + 1;
        SceneManager.LoadScene(nsi);
        print("Scene was changed to scene index: " + nsi);
    }
    
    private void loadMainMenu()
    {
        SceneManager.LoadScene(0);
        print("Scene was changed to Main Menu: ");
    }

    private void loadLostScene()
    {
        SceneManager.LoadScene(1);
        print("Scene was changed to Loser Scene");
    }
    
    private bool noMoreBacteria()
    {
        List<GameObject> bacteriaSmall = BacteriaSmallManager.Instance.pooledBacterias;
        List<GameObject> bacteriaLarge = BacteriaLargeManager.Instance.pooledBacterias;

        GameObject[] bacteria = bacteriaLarge.Concat(bacteriaSmall).ToArray();

        bool allDead = true;
        foreach (GameObject bacterium in bacteria)
        {
            if (bacterium.activeInHierarchy)
            {
                allDead = false;
            }
        }
            
        if (
            allDead && 
            bacteriaSpawner.numberOfSmallBacteriaToSpawn < 1 && 
            bacteriaSpawner.numberOfLargeBacteriaToSpawn < 1)
        {
            print("YOU WON!");
            return true;
        }
        return false;
    }
    
    private bool noMoreCells()
    {
        List<GameObject> cells = CellManager.Instance.cellList;

        GameObject[] cell = cells.ToArray();
            
        if (!cell.Any() )
        {
            print("YOU LOST!");
            return true;
        }
        return false;
    }
}
