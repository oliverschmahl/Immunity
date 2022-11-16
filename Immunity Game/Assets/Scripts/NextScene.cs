using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NextScene : MonoBehaviour
{
    public string nextScene;
    // Start is called before the first frame update
    void Start()
    {
        ExecuteNextScene(nextScene);
    }

    public void ExecuteNextScene(string next) {
        SceneManager.LoadScene(next);
    }
    
    
}
