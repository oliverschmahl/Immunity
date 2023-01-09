using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBar : MonoBehaviour
{

    public float maxHealth;
    public float currentHealth;
    private float originalScale;
    private Health health;
    
    // Start is called before the first frame update
    void Start()
    {
        //gets the scale of healthBar
        originalScale = gameObject.transform.localScale.x; 
        
        //retrieves the Health script from parent and extracts the max health
        health = gameObject.GetComponentInParent(typeof(Health)) as Health;
    }

    // Update is called once per frame
    void Update()
    {
        maxHealth = health.GetMaxHealth();
        currentHealth = health.GetCurrentHealth();
        Vector3 tmpScale = gameObject.transform.localScale;
        tmpScale.x = currentHealth / maxHealth * originalScale;
        gameObject.transform.localScale = tmpScale;
    }
}