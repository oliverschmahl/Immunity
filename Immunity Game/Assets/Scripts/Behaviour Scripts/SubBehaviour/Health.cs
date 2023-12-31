using System;
using System.Collections;
using System.Collections.Generic;
using Behaviour_Scripts;
using Managers;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] private int health = 100;
    private float maxHealth;

    public void Start()
    {
        maxHealth = (float) health;
    }

    public void TakeDamage(int amount) {
        if(amount < 0) {
            throw new System.ArgumentException("Damage cannot be negative");
        }
        
        switch (tag)
        {
            case "Bacteria Small":
                gameObject.GetComponent<Bacteria>().DamageFlash();
                break;
            case "Bacteria Large":
                gameObject.GetComponent<Bacteria>().DamageFlash();
                break;
        }
        
        health -= amount;
        

        if(health <= 0)
        {
            switch (tag)
            {
                case "Cell":
                    CellManager.Instance.RemoveCell(gameObject);
                    Debug.Log("Cell removed");
                    break;
                case "Bacteria Small":
                    BacteriaSmallManager.Instance.RemoveBacteria(gameObject);
                    Debug.Log("Small Bacteria removed");
                    break;
                case "Bacteria Large":
                    BacteriaLargeManager.Instance.RemoveBacteria(gameObject);
                    Debug.Log("Large Bacteria removed");
                    break;
                case "T Cell":
                    TcellManager.Instance.RemoveTcell(gameObject);
                    Debug.Log("T Cell removed");
                    break;
                case "Macrophage":
                    MacrophageManager.Instance.RemoveMacrophage(gameObject);
                    Debug.Log("Macrophage removed");
                    break;
                case "Antibodies":
                    AntibodiesManager.Instance.RemoveAntibodies(gameObject);
                    Debug.Log("Antibodies removed");
                    break;
                case "Complement Protein":
                    ComplementProteinManager.Instance.Remove(gameObject);
                    Debug.Log("Complement Protein removed");
                    break;
            }
        }
    }

    public float GetMaxHealth()
    {
        return maxHealth;
    }

    public float GetCurrentHealth()
    {
        return (float) health;
    }
}

