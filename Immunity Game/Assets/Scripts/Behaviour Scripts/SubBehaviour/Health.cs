using System.Collections;
using System.Collections.Generic;
using Managers;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] private int health = 100;

    public void TakeDamage(int amount) {
        if(amount < 0) {
            throw new System.ArgumentException("Damage cannot be negative");
        }
        
        health -= amount;

        if(health <= 0)
        {
            switch (tag)
            {
                case "Cell": 
                    CellManager.Instance.RemoveCell(gameObject);
                    break;
            }
        }
    }
}
