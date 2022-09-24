using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField] private int damage = 5;

    [SerializeField] private float speed = 1.5f;

    [SerializeField] private EnemyData data;
    
    [SerializeField] private GameObject defenseOrganism; //Todo: this should not be serializefield. It should be set in start().
    
    void Start()
    {
        defenseOrganism = GameObject.FindWithTag("DefenceOrganism");
        Debug.Log("DefenseOrganism: " + defenseOrganism.name);
        setEnemyValues();
    }

    // Update is called once per frame
    void Update()
    {
        BacteriaSmall();
    }

    private void BacteriaSmall()
    {
        Debug.Log("Bacteria is moving");
        transform.position =
            Vector2.MoveTowards(transform.position, defenseOrganism.transform.position, speed * Time.deltaTime); //TODO: Use tobias' move method
    }


    private void OnTriggerEnter2D(Collider2D col) 
    {
        Collider2D collidingInstance = col;

        if (!collidingInstance.CompareTag("DefenseOrganism")) return;
        if (collidingInstance.GetComponent<Health>() == null) return;
        collidingInstance.GetComponent<Health>().Damage(damage);
        Debug.Log(collidingInstance + " Is attacking");
    }

    private void setEnemyValues()
    {
        GetComponent<Health>().SetHealth(data.hp);
        damage = data.damage;
        speed = data.speed;
    }
}
