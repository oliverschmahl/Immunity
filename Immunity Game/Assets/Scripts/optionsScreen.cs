using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Audio;

public class OptionsScreen : MonoBehaviour
{
    public Toggle fullscreenToggle, vsyncToggle;
    public List<ResItem> resolutions = new List<ResItem>();
    private int _selectedResolution;
    public TMP_Text resolutionLabel;
    public AudioMixer theMixer;
    public TMP_Text masterLabel;
    public Slider masterSlider;
    
    
    // Start is called before the first frame update
    void Start()
    {
        fullscreenToggle.isOn = Screen.fullScreen;

        if (QualitySettings.vSyncCount == 0)
        {
            vsyncToggle.isOn = false;
        }
        else
        {
            vsyncToggle.isOn = true;
        }

        bool foundRes = false;
        for (int i = 0; i < resolutions.Count; i++) {
            if (Screen.width == resolutions[i].horizontal && Screen.height == resolutions[i].vertical) {
                foundRes = true;

                _selectedResolution = i;
                
                UpdateResLabel();
            }
        }

        if (!foundRes)
        {
            ResItem newRes = new ResItem();
            newRes.horizontal = Screen.width;
            newRes.vertical = Screen.height;
            
            resolutions.Add(newRes);
            _selectedResolution = resolutions.Count - 1;
            
            UpdateResLabel();
        }

        float vol = 0f;
        theMixer.GetFloat("MasterVol", out vol);
        masterSlider.value = vol;
        masterLabel.text = Mathf.RoundToInt(masterSlider.value + 80).ToString();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ResLeft()
    {
        _selectedResolution--;
        if (_selectedResolution < 0)
        {
            _selectedResolution = 0;
        }
        UpdateResLabel();
    }

    public void ResRight()
    {
        _selectedResolution++;
        if (_selectedResolution > resolutions.Count - 1)
        {
            _selectedResolution = resolutions.Count - 1;
        }
        UpdateResLabel();
    }

    public void UpdateResLabel()
    {
        resolutionLabel.text = resolutions[_selectedResolution].horizontal.ToString() + " x " + resolutions[_selectedResolution].vertical.ToString();
    }

    public void ApplyGraphics()
    {
        Screen.fullScreen = fullscreenToggle.isOn;

        if (vsyncToggle.isOn)
        {
            QualitySettings.vSyncCount = 1;
        }
        else
        {
            QualitySettings.vSyncCount = 0;
        }
        
        Screen.SetResolution(resolutions[_selectedResolution].horizontal, resolutions[_selectedResolution].vertical, fullscreenToggle.isOn);
    }

    public void SetMasterVolume()
    {
        masterLabel.text = Mathf.RoundToInt(masterSlider.value + 80).ToString();
        theMixer.SetFloat("MasterVol", masterSlider.value);
        
        PlayerPrefs.SetFloat("MasterVol", masterSlider.value);
    }
}

[System.Serializable]
public class ResItem
{
    public int horizontal, vertical;
}
