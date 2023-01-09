using System;
using System.Collections;
using System.Globalization;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ButtonCooldown : MonoBehaviour
{
    private Button button;
    private TMP_Text timerText;
    [SerializeField] float cooldownDuration = 5f;
 
    void Awake()
    {
        // Get a reference to your button
        button = GetComponent<Button>();
        if (button != null)
        {
            // Listen to its onClick event
            button.onClick.AddListener(OnButtonClick);
            timerText = GetComponentInChildren<TMP_Text>();
        }
    }

    // method is called whenever myButton is pressed
    void OnButtonClick()
    {
        StartCoroutine(Cooldown());
    }
 
    // Coroutine that will deactivate and reactivate the button 
    IEnumerator Cooldown()
    {
        // Deactivate myButton
        button.interactable = false;
        
        for (float i = 0; i < cooldownDuration; i++)
        {
            timerText.text = (cooldownDuration - i).ToString();
            yield return new WaitForSeconds(1);
        }
        // Reactivate myButton
        timerText.text = "";
        button.interactable = true;
    }
}
