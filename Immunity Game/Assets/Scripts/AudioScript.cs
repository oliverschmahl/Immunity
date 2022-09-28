using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioScript : MonoBehaviour
{
    private static AudioScript instance = null;
    
    public static AudioScript Instance
    {
        get { return instance; }
    }
    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Debug.Log("Scene changed");
        instance = this;
        DontDestroyOnLoad(gameObject);
    }
}
