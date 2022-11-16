using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StopAudioScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        try {
            bool audioScriptExists = AudioScript.Instance.gameObject != null;
            if (audioScriptExists)
            {
                Destroy(AudioScript.Instance.gameObject);
            }
        }       
        catch (NullReferenceException ex) {
            Debug.Log("There is no AudioScript to stop");
        }
        
        
    }
}
