using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] private int health = 100;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
       
    }

    public void Damage(int amount) {
        if(amount < 0) {
            throw new System.ArgumentException("Damage cannot be negative");
        }
        this.health -= amount;

        if(health <= 0) {
            Die();
        }
    }

    public void SetHealth(int health)
    {
        this.health = health;
    }

    private void Die() {
        Debug.Log("You killed me ");
        Destroy(gameObject);
    }
    
}
