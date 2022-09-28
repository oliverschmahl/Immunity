using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StopAudioScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        if (AudioScript.Instance.gameObject)
        {
            Destroy(AudioScript.Instance.gameObject);
        }
    }

}
